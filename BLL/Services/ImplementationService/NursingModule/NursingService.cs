using AutoMapper;
using BLL.Dtos.Nursing;
using BLL.Services.AbstractServices;
using BLL.Services.AbstractServices.Users;
using DAL.Models.NursingModule;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.NursingSpecs;
using DAL.Exceptions;

namespace BLL.Services.ImplementationService.NursingModule
{
    public class NursingService
        (IUnitOfWork _unitOfWork, IMapper _mapper, INotificationService _notificationService) : INursingService
    {
       
        public async Task<NursingRequestDto> RequestNursingAsync(int patientId, CreateNursingRequestDto dto)
        {
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

        public async Task<IEnumerable<NursingRequestDto>> GetMyNursingRequestsAsync(int patientId)
        {
            var requests = await _unitOfWork.GetRepository<NursingRequest>()
                .GetAllAsync(new NursingRequestsByPatientIdSpec(patientId));

            if (!requests.Any())
                return Enumerable.Empty<NursingRequestDto>();

            return _mapper.Map<IEnumerable<NursingRequestDto>>(requests);
        }

        public async Task<IEnumerable<NursingRequestDto>> GetNurseRequestsAsync(int nurseId)
        {
            var requests = await _unitOfWork.GetRepository<NursingRequest>()
                .GetAllAsync(new NursingRequestsByNurseIdSpec(nurseId));

            if (!requests.Any())
                return Enumerable.Empty<NursingRequestDto>();

            return _mapper.Map<IEnumerable<NursingRequestDto>>(requests);
        }

        public async Task<NursingRequestDto> UpdateNursingStatusAsync(int requestId, UpdateNursingStatusDto dto, int userId)
        {
            var request = (await _unitOfWork.GetRepository<NursingRequest>()
                .GetAllAsync(new NursingRequestByIdSpec(requestId)))
                .FirstOrDefault()
                ?? throw new NursingRequestNotFoundException(requestId);

            if (request.NurseId != userId)
                throw new UnauthorizedException("Only the assigned nurse can update this request.");

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
            var request = (await _unitOfWork.GetRepository<NursingRequest>()
                .GetAllAsync(new NursingRequestByIdSpec(requestId)))
                .FirstOrDefault()
                ?? throw new NursingRequestNotFoundException(requestId);

            if (request.PatientId != userId)
                throw new UnauthorizedException("You are not authorized to cancel this request.");

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
                throw new UnauthorizedException("You can only review your own nursing requests.");

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

        public async Task<NursingReviewDto?> GetNursingReviewAsync(int requestId)
        {
            var request = await _unitOfWork.GetRepository<NursingRequest>().GetByIdAsync(requestId)
                ?? throw new NursingRequestNotFoundException(requestId);

            if (request.Review is null)
                return null;

            return _mapper.Map<NursingReviewDto>(request.Review);
        }
    }
}