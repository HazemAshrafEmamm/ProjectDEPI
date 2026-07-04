using AutoMapper;
using BLL.Dtos.Schedule;
using BLL.Services.AbstractServices.AppointmentModule;
using DAL.Models.AppointmentModule;
using DAL.Repository;
using DAL.Specifications;
using DAL.Specifications.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.AppointmentModule
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenaricRepository<DoctorSchedule> _scheduleRepo;

        public DoctorScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduleRepo = _unitOfWork.GetRepository<DoctorSchedule>();
        }

        public async Task<DoctorScheduleDto> CreateScheduleAsync(int doctorId, CreateDoctorScheduleDto dto)
        {
            // Check for duplicate schedule
            var spec = new GetExistingScheduleSpecs(doctorId, dto.DayOfWeek, dto.StartTime, dto.EndTime);
              
            
            var existing = await _scheduleRepo.GetAllAsync(spec);
            if (existing.Any())
                throw new InvalidOperationException("This schedule already exists.");

            var schedule = _mapper.Map<DoctorSchedule>(dto);
            schedule.DoctorId = doctorId;
            
            await _scheduleRepo.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DoctorScheduleDto>(schedule);
        }

        public async Task<bool> DeleteScheduleAsync(int scheduleId, int doctorId)
        {
            var schedule = await _scheduleRepo.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {scheduleId} not found.");

            if (schedule.DoctorId != doctorId)
                throw new UnauthorizedAccessException("You are not authorized to delete this schedule.");

            _scheduleRepo.Delete(schedule);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<DoctorScheduleDto>> GetDoctorSchedulesAsync(int doctorId)
        {
            var spec = new BaseSpecification<DoctorSchedule>(s => s.DoctorId == doctorId);
            var schedules = await _scheduleRepo.GetAllAsync(spec);
            return _mapper.Map<IEnumerable<DoctorScheduleDto>>(schedules);
        }

        public async Task<DoctorScheduleDto> UpdateScheduleAsync(int scheduleId, UpdateDoctorScheduleDto dto, int doctorId)
        {
            var schedule = await _scheduleRepo.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {scheduleId} not found.");

            if (schedule.DoctorId != doctorId)
                throw new UnauthorizedAccessException("You are not authorized to update this schedule.");

            _mapper.Map(dto, schedule);

            _scheduleRepo.Update(schedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DoctorScheduleDto>(schedule);
        }
    }
}
