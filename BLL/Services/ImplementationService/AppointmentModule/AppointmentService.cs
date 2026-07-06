using AutoMapper;
using BLL.Dtos.Appointment;
using BLL.Services.AbstractServices.AppointmentModule;
using DAL.Exceptions;
using DAL.Models.AppointmentModule;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.AppointmentModule
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenaricRepository<Appointment> _repo;
        private readonly IGenaricRepository<DoctorSchedule> _scheduleRepo;
        private readonly IUserRepository _userRepository;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Appointment>();
            _scheduleRepo = _unitOfWork.GetRepository<DoctorSchedule>();
            _userRepository = userRepository;
        }

        #region Public Methods
        
        public async Task<AppointmentDto> BookAppointmentAsync(int patientId, CreateAppointmentDto dto)
        {
            await ValidatePatientAsync(patientId);
            ValidateFutureDate(dto.AppointmentDate);

            var schedule = await ValidateScheduleAsync(dto.ScheduleId, dto.DoctorId);
            ValidateDayOfWeek(dto.AppointmentDate, schedule.DayOfWeek);
            await ValidateSlotAvailabilityAsync(dto.DoctorId, dto.AppointmentDate, dto.ScheduleId);

            var appointmentEntity = _mapper.Map<Appointment>(dto);
            appointmentEntity.PatientId = patientId;
            appointmentEntity.AppointmentTime = schedule.StartTime;
            appointmentEntity.Status = AppointmentStatus.Pending;
            appointmentEntity.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(appointmentEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointmentEntity);
        }

        public async Task<AppointmentDto> CancelAppointmentAsync(int appointmentId, int userId)
        {
            var appointment = await GetAppointmentOrThrowAsync(appointmentId);
            ValidateOwnership(appointment, userId);

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException($"Appointment with ID {appointmentId} is already cancelled.");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Cannot cancel a completed appointment.");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;
            _repo.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId, int doctorId)
        {
            var appointment = await GetAppointmentOrThrowAsync(appointmentId);

            if (appointment.DoctorId != doctorId)
                throw new UnauthorizedAccessException("You are not authorized to confirm this appointment.");

            if (appointment.Status != AppointmentStatus.Pending)
                throw new InvalidOperationException($"Only pending appointments can be confirmed. Current status: {appointment.Status}.");

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.UpdatedAt = DateTime.UtcNow;
            _repo.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> CompleteAppointmentAsync(int appointmentId, int doctorId)
        {
            var appointment = await GetAppointmentOrThrowAsync(appointmentId);

            if (appointment.DoctorId != doctorId)
                throw new UnauthorizedAccessException("You are not authorized to complete this appointment.");

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new InvalidOperationException($"Only confirmed appointments can be completed. Current status: {appointment.Status}.");

            appointment.Status = AppointmentStatus.Completed;
            appointment.UpdatedAt = DateTime.UtcNow;
            _repo.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> GetAppointmentAsync(int appointmentId, int userId)
        {
            var appointment = await GetAppointmentOrThrowAsync(appointmentId);
            ValidateOwnership(appointment, userId);
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<AvailableDoctorSlotDto>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var spec = new GetAvailableSlotsSpecs(doctorId, date);
            var doctorSchedules = await _scheduleRepo.GetAllAsync(spec);

            var appointmentSpec = new AppointmentNotCancelledSpec(doctorId, date);
            var bookedAppointments = await _repo.GetAllAsync(appointmentSpec);
            var bookedScheduleIds = bookedAppointments.Select(a => a.ScheduleId).ToHashSet();

            var freeSchedules = doctorSchedules.Where(s => !bookedScheduleIds.Contains(s.Id));
            var dtos = _mapper.Map<IEnumerable<AvailableDoctorSlotDto>>(freeSchedules);
            foreach (var dto in dtos)
            {
                dto.AvailableDates = new List<DateTime> { date.Date };
            }
            return dtos;
        }

        public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId)
        {
            var spec = new AppointmentsDoctorSpec(doctorId);
            var appointments = await _repo.GetAllAsync(spec);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetMyAppointmentsAsync(int userId)
        {
            var spec = new AppointmentsPatientSpec(userId);
            var appointments = await _repo.GetAllAsync(spec);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto, int userId)
        {
            var appointment = await GetAppointmentOrThrowAsync(appointmentId);
            ValidateOwnership(appointment, userId);

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Cannot update a cancelled appointment.");

            if (appointment.AppointmentDate < DateTime.Now)
                throw new InvalidOperationException("Cannot update a past appointment.");

            // If schedule is being changed, validate it
            if (dto.ScheduleId.HasValue)
            {
                var schedule = await ValidateScheduleAsync(dto.ScheduleId.Value, appointment.DoctorId);
                appointment.AppointmentTime = schedule.StartTime;
            }

            // If date is being changed, validate it
            if (dto.AppointmentDate.HasValue)
            {
                ValidateFutureDate(dto.AppointmentDate.Value);

                var scheduleId = dto.ScheduleId ?? appointment.ScheduleId;
                var currentSchedule = await _scheduleRepo.GetByIdAsync(scheduleId);
                if (currentSchedule != null)
                    ValidateDayOfWeek(dto.AppointmentDate.Value, currentSchedule.DayOfWeek);
            }

            // Check slot availability if schedule or date changed
            if (dto.ScheduleId.HasValue || dto.AppointmentDate.HasValue)
            {
                var checkDate = dto.AppointmentDate ?? appointment.AppointmentDate;
                var checkScheduleId = dto.ScheduleId ?? appointment.ScheduleId;
                await ValidateSlotAvailabilityAsync(appointment.DoctorId, checkDate, checkScheduleId, appointmentId);
            }

            _mapper.Map(dto, appointment);
            appointment.UpdatedAt = DateTime.UtcNow;

            _repo.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AppointmentDto>(appointment);
        }

        #endregion

        #region Private Validation Methods

        private async Task<Appointment> GetAppointmentOrThrowAsync(int appointmentId)
        {
            var appointment = await _repo.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
            return appointment;
        }

        private static void ValidateOwnership(Appointment appointment, int userId)
        {
            if (appointment.PatientId != userId && appointment.DoctorId != userId)
                throw new UnauthorizedAccessException("You are not authorized to access this appointment.");
        }

        private async Task ValidatePatientAsync(int patientId)
        {
            var patient = await _userRepository.GetPatientWithAppointmentAsync(patientId)
                ?? throw new PatientNotFoundException(patientId);
            if (patient.UserType != "Patient")
                throw new UnauthorizedAccessException("Only patients can book appointments.");
        }

        private async Task<DoctorSchedule> ValidateScheduleAsync(int scheduleId, int doctorId)
        {
            var schedule = await _scheduleRepo.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {scheduleId} not found.");

            if (schedule.DoctorId != doctorId)
                throw new InvalidOperationException("The selected schedule does not belong to the specified doctor.");

            if (!schedule.IsAvailable)
                throw new InvalidOperationException("The selected schedule is not available.");

            return schedule;
        }

        private async Task ValidateSlotAvailabilityAsync(int doctorId, DateTime date, int scheduleId, int? excludeAppointmentId = null)
        {
            var appointmentSpec = new AppointmentNotCancelledSpec(doctorId, date);
            var bookedAppointments = await _repo.GetAllAsync(appointmentSpec);
            var isSlotTaken = bookedAppointments.Any(a => a.ScheduleId == scheduleId && a.Id != excludeAppointmentId);
            if (isSlotTaken)
                throw new InvalidOperationException("This time slot is already booked.");
        }

        private static void ValidateFutureDate(DateTime date)
        {
            if (date.Date < DateTime.Today)
                throw new InvalidOperationException("Cannot book an appointment in the past.");
        }

        private static void ValidateDayOfWeek(DateTime date, DayOfWeek expectedDay)
        {
            if (date.DayOfWeek != expectedDay)
                throw new InvalidOperationException($"The appointment date must be on {expectedDay}.");
        }

        #endregion
    }
}
