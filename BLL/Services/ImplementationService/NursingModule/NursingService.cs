using AutoMapper;
using BLL.Dtos.Consultion;
using BLL.Dtos.Nursing;
using BLL.Services.AbstractServices;
using BLL.Services.AbstractServices.Users;
using DAL.Exceptions;
using DAL.Exceptions.NursingModule;
using DAL.Models.NursingModule;
using DAL.Models.Users;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.NursingRequestSpecs;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.ImplementationService.NursingModule
{
    public class NursingService(IUnitOfWork _unitOfWork, IMapper _mapper, INotificationService _notificationService, IUserRepository _userRepository, UserManager<ApplicationUser> _userManager) : INursingService
    {

        public async Task<IEnumerable<NurseInfoDto>> SearchNursesAsync(SearchNurseDto searchDto)
        {
            var nurses = await _userRepository.SearchNursesAsync(
                searchDto.Name,
                searchDto.Specialization,
                searchDto.PageNumber,
                searchDto.PageSize);


            return _mapper.Map<IEnumerable<NurseInfoDto>>(nurses);
        }

        public async Task<NursingRequestDto> RequestNursingAsync(int patientId, CreateNursingRequestDto dto)
        {
            // Validate nurse exists
            var nurse = await _userManager.FindByIdAsync(dto.NurseId.ToString())
                        as Nurse
                        ?? throw new NurseNotFoundException(dto.NurseId);

            var request = new NursingRequest
            {
                PatientId = patientId,
                NurseId = dto.NurseId,
                CareType = dto.CareType,
                Status = "Pending"
            };

            await _unitOfWork.GetRepository<NursingRequest>().AddAsync(request);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                $"You have a new nursing request from patient {patientId}.",
                NotificationType.System,
                dto.NurseId
            );

            return _mapper.Map<NursingRequestDto>(request);
        }

        public async Task<IEnumerable<NursingRequestDto>> GetMyNursingRequestsAsync(int requesterId)
        {
            var myRequests = await _unitOfWork.GetRepository<NursingRequest>().GetAllAsync(new MyNursingRequestsSpecs(requesterId));
            if (myRequests is null || !myRequests.Any())
                return Enumerable.Empty<NursingRequestDto>();

            return _mapper.Map<IEnumerable<NursingRequestDto>>(myRequests);

        }

        public async Task<NursingRequestDto> UpdateNursingStatusAsync(int requestId, UpdateNursingStatusDto dto, int userId)
        {
            var request = (await _unitOfWork.GetRepository<NursingRequest>()
                .GetAllAsync(new NursingRequestByIdSpec(requestId)))
                .FirstOrDefault()
                ?? throw new NursingRequestNotFoundException(requestId);

            if (request.NurseId != userId)
                throw new UnauthorizedNursingAccessException(userId, requestId);

            if (request.Status == "Cancelled" || request.Status == "Completed")
                throw new BadRequestException([$"Cannot update a request with status '{request.Status}'."]);

            request.Status = dto.Status;

            _unitOfWork.GetRepository<NursingRequest>().Update(request);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                $"Your nursing request status has been updated to '{dto.Status}'.",
                NotificationType.System,
                request.PatientId
            );

            return _mapper.Map<NursingRequestDto>(request);
        }

        public async Task<NursingRequestDto> CancelNursingAsync(int requestId, int userId)
        {
            var request = await _unitOfWork.GetRepository<NursingRequest>().GetByIdAsync(requestId)
                ?? throw new NursingRequestNotFoundException(requestId);

            if (request.PatientId != userId)
                throw new UnauthorizedNursingAccessException(userId, requestId);

            if (request.Status != "Pending")
                throw new BadRequestException([$"Only pending requests can be cancelled. Current status: '{request.Status}'."]);

            request.Status = "Cancelled";

            _unitOfWork.GetRepository<NursingRequest>().Update(request);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                "Your nursing request has been cancelled.",
                NotificationType.System,
                request.NurseId
            );

            return _mapper.Map<NursingRequestDto>(request);
        }

        public async Task<NursingReviewDto> AddNursingReviewAsync(int requestId, int patientId, CreateNursingReviewDto dto)
        {
            var request = (await _unitOfWork.GetRepository<NursingRequest>()
                .GetAllAsync(new NursingRequestByIdSpec(requestId)))
                .FirstOrDefault()
                ?? throw new NursingRequestNotFoundException(requestId);

            if (request.PatientId != patientId)
                throw new UnauthorizedNursingAccessException(patientId, requestId);

            if (request.Status != "Completed")
                throw new BadRequestException(["You can only review completed nursing requests."]);

            if (request.Review != null)
                throw new BadRequestException(["A review already exists for this request."]);

            var review = new NursingReview
            {
                NursingRequestId = requestId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await _unitOfWork.GetRepository<NursingReview>().AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                $"You received a new review with rating {dto.Rating}/5.",
                NotificationType.System,
                request.NurseId
            );

            return _mapper.Map<NursingReviewDto>(review);
        }

        public async Task<IEnumerable<NursingReviewDto>?> GetNursingReviewAsync(int requestId)
        {
            // Validate request exists
            _ = await _unitOfWork.GetRepository<NursingRequest>().GetByIdAsync(requestId)
                ?? throw new NursingRequestNotFoundException(requestId);

            var review = await _unitOfWork.GetRepository<NursingReview>().GetAllAsync(new ReviewByRequestIdSpecs(requestId));

            return _mapper.Map<IEnumerable<NursingReviewDto>>(review);
        }

        public async Task<IEnumerable<NursingReviewDto>?> GetNursingReviewByNurseIdAsync(int nurseId)
        {
            //validate
            var nurse = await _userManager.FindByIdAsync(nurseId.ToString())
                        as Nurse
                        ?? throw new NurseNotFoundException(nurseId);
            var nurseReviews = await _unitOfWork.GetRepository<NursingReview>().GetAllAsync(new ReviewByNurseIdSpecs(nurseId));

            if (nurseReviews is null || !nurseReviews.Any())
                return Enumerable.Empty<NursingReviewDto>();

            return _mapper.Map<IEnumerable<NursingReviewDto>>(nurseReviews);
        }
    }
}
