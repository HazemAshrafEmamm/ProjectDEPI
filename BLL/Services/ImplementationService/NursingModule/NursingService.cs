using AutoMapper;
using BLL.Dtos.Consultion;
using BLL.Dtos.Nursing;
using BLL.Services.AbstractServices.Users;
using DAL.Exceptions;
using DAL.Exceptions.NursingModule;
using DAL.Models.NursingModule;
using DAL.Models.Users;
using DAL.Repository;
using DAL.Specifications.NursingRequestSpecs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.NursingModule
{
    public class NursingService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        UserManager<ApplicationUser> _userManager,
        IUserRepository _userRepository) : INursingService
    {
        private readonly IGenaricRepository<NursingRequest> _requestRepo
            = _unitOfWork.GetRepository<NursingRequest>();

        private readonly IGenaricRepository<NursingReview> _reviewRepo
            = _unitOfWork.GetRepository<NursingReview>();

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

            await _requestRepo.AddAsync(request);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NursingRequestDto>(request);
        }

        public async Task<IEnumerable<NursingRequestDto>> GetMyNursingRequestsAsync(int requesterId)
        {
            var myRequests = await _requestRepo.GetAllAsync(new MyNursingRequestsSpecs(requesterId));
            if (myRequests is null || !myRequests.Any())
                return Enumerable.Empty<NursingRequestDto>();

            return _mapper.Map<IEnumerable<NursingRequestDto>>(myRequests);
        }

        public async Task<NursingRequestDto> UpdateNursingStatusAsync(int requestId, UpdateNursingStatusDto dto, int userId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId)
                          ?? throw new NursingRequestNotFoundException(requestId);

            // Only the nurse assigned or the patient can update
            if (request.PatientId != userId && request.NurseId != userId)
                throw new UnauthorizedNursingAccessException(userId, requestId);

            request.Status = dto.Status;
            _requestRepo.Update(request);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NursingRequestDto>(request);
        }

        public async Task<NursingRequestDto> CancelNursingAsync(int requestId, int userId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId)
                          ?? throw new NursingRequestNotFoundException(requestId);

            if (request.PatientId != userId && request.NurseId != userId)
                throw new UnauthorizedNursingAccessException(userId, requestId);

            if (request.Status != "Pending")
                throw new NursingRequestNotCancelableException(requestId);

            request.Status = "Cancelled";
            _requestRepo.Update(request);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NursingRequestDto>(request);
        }

        public async Task<NursingReviewDto> AddNursingReviewAsync(int requestId, int patientId, CreateNursingReviewDto dto)
        {
            var request = await _requestRepo.GetByIdAsync(requestId)
                          ?? throw new NursingRequestNotFoundException(requestId);

            if (request.PatientId != patientId)
                throw new UnauthorizedNursingAccessException(patientId, requestId);

            if (request.Status != "Completed")
                throw new NursingRequestNotCompletedException(requestId);

            var existingReview = await _reviewRepo.GetAllAsync(new ReviewByRequestIdSpecs(requestId));
            if (existingReview.Any())
                throw new NursingReviewAlreadyExistsException(requestId);

            var review = new NursingReview
            {
                NursingRequestId = requestId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await _reviewRepo.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NursingReviewDto>(review);
        }

        public async Task<IEnumerable<NursingReviewDto>?> GetNursingReviewAsync(int requestId)
        {
            // Validate request exists
            _ = await _requestRepo.GetByIdAsync(requestId)
                ?? throw new NursingRequestNotFoundException(requestId);

            var review = await _reviewRepo.GetAllAsync(new ReviewByRequestIdSpecs(requestId));

            return _mapper.Map<IEnumerable<NursingReviewDto>>(review);
        }

        public async Task<IEnumerable<NursingReviewDto>?> GetNursingReviewByNurseIdAsync(int nurseId)
        {
            //validate
            var nurse = await _userManager.FindByIdAsync(nurseId.ToString())
                        as Nurse
                        ?? throw new NurseNotFoundException(nurseId);
            var nurseReviews = await _reviewRepo.GetAllAsync(new ReviewByNurseIdSpecs(nurseId));

            if (nurseReviews is null || !nurseReviews.Any())
                return Enumerable.Empty<NursingReviewDto>();

            return _mapper.Map<IEnumerable<NursingReviewDto>>(nurseReviews);
        }
    }
}

