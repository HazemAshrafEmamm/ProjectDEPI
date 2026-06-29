using AutoMapper;
using BLL.Dtos.Appointment;
using BLL.Dtos.Medication;
using BLL.Services.AbstractServices;
using BLL.Services.AbstractServices.AppointmentModule;
using DAL.Exceptions;
using DAL.Models.AppointmentModule;
using DAL.Models.OrderModule;
using DAL.Models.Users;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.AppointmentModule
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenaricRepository<Appointment> _repo;
        private readonly IAttachmentService _attach;
        private readonly IUserRepository _userRepository;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper,  IAttachmentService attach, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Appointment>();
            _attach = attach;
            _userRepository = userRepository;
        }

        public async Task<AppointmentDto> BookAppointmentAsync(int patientId, CreateAppointmentDto dto)
        {
            var ispatient = await ValidatePatient(patientId);
            if (!ispatient)
                throw new KeyNotFoundException("Patient not found or unauthorized to book an appointment.");
            var AppointmentEntity = _mapper.Map<Appointment>(dto);
            AppointmentEntity.PatientId = patientId;
            await _repo.AddAsync(AppointmentEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AppointmentDto>(AppointmentEntity);

        }

        public async Task<AppointmentDto> CancelAppointmentAsync(int appointmentId, int userId)
        {
            var appointment = await _repo.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
            }
            if (appointment.PatientId != userId && appointment.DoctorId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to cancel this appointment.");
            }
            if (appointment.Status == AppointmentStatus.Cancelled|| appointment.Status == AppointmentStatus.Completed)
            {
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} already cancelled");
            }
            appointment.Status = AppointmentStatus.Cancelled;
            _repo.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> GetAppointmentAsync(int appointmentId, int userId)
        {
            var appointment = await _repo.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
            }
            if (appointment.PatientId != userId && appointment.DoctorId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this appointment.");
            }
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<AvailableDoctorSlotDto>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var spec=new GetAvailableSlotsSpecs(doctorId, date);
            var doctorSchedules = await _unitOfWork.GetRepository<DoctorSchedule>().GetAllAsync(spec);
            var appointmentSpec = new AppointmentNotCancelledSpec(doctorId, date);
            var bookedAppointments = await _repo.GetAllAsync(appointmentSpec);
            var bookedScheduleIds = bookedAppointments.Select(a => a.ScheduleId).ToList();

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
            var appointment = await _repo.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
            }
            if (appointment.PatientId != userId && appointment.DoctorId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this appointment.");
            }
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot update a cancelled appointment.");
            }
            if (appointment.AppointmentDate < DateTime.Now)
            {
                throw new InvalidOperationException("Cannot update a past appointment.");
            }

   
            _mapper.Map(dto, appointment);

            _repo.Update(appointment); 
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AppointmentDto>(appointment);
        }
        private async Task<bool> ValidatePatient(int Id)
        {
            var patient = await _userRepository.GetPatientWithAppointmentAsync(Id)
                ?? throw new PatientNotFoundException(Id);
            if (patient.UserType == "Patient")
                return true;
            return false;
        }
    }
}
