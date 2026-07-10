using AutoMapper;
using BLL.Dtos.Admin;
using BLL.Services.AbstractServices.Admin;
using DAL.Exceptions;
using DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ImplementationService.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly TabibyDbContext _dbContext;

        public AdminService(UserManager<ApplicationUser> userManager, IMapper mapper, TabibyDbContext dbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<AdminUserDto> CreateDoctorAsync(CreateDoctorAdminDto dto)
        {
            await EnsureEmailUniqueAsync(dto.Email);

            var doctor = new Doctor
            {
                Fullname = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.UserName ?? dto.Email.Split('@')[0],
                PhoneNumber = dto.PhoneNumber,
                UserType = "Doctor",
                IsActive = true,
                Specialty = dto.Specialty,
                Location = dto.Location ?? string.Empty
            };

            return await CreateUserAndAssignRoleAsync(doctor, dto.Password, "Doctor");
        }

        public async Task<AdminUserDto> CreateNurseAsync(CreateNurseAdminDto dto)
        {
            await EnsureEmailUniqueAsync(dto.Email);

            var nurse = new Nurse
            {
                Fullname = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.UserName ?? dto.Email.Split('@')[0],
                PhoneNumber = dto.PhoneNumber,
                UserType = "Nurse",
                IsActive = true,
                Specialization = dto.Specialization ?? string.Empty
            };

            return await CreateUserAndAssignRoleAsync(nurse, dto.Password, "Nurse");
        }

        public async Task<AdminUserDto> CreatePharmacistAsync(CreatePharmacistAdminDto dto)
        {
            await EnsureEmailUniqueAsync(dto.Email);

            var pharmacist = new Pharmacist
            {
                Fullname = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.UserName ?? dto.Email.Split('@')[0],
                PhoneNumber = dto.PhoneNumber,
                UserType = "Pharmacist",
                IsActive = true,
                PharmacyName = dto.PharmacyName
            };

            return await CreateUserAndAssignRoleAsync(pharmacist, dto.Password, "Pharmacist");
        }

        public async Task<AdminUserDto> DeleteUserAsync(int userId, int requestingAdminId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserByIdNotFoundException(userId);

            if (!user.IsActive)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "User is already deactivated." });

            if (userId == requestingAdminId)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Admins cannot deactivate themselves." });

            var isTargetAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isTargetAdmin)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Cannot deactivate another Admin account." });

            user.IsActive = false;
            
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            var mappedUser = _mapper.Map<AdminUserDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            mappedUser.Roles = roles.ToList();

            if (user is Doctor doc)
            {
                mappedUser.Specialty = doc.Specialty;
                mappedUser.Location = doc.Location;
            }
            else if (user is Nurse nurse)
            {
                mappedUser.Specialization = nurse.Specialization;
            }
            else if (user is Pharmacist pharm)
            {
                mappedUser.PharmacyName = pharm.PharmacyName;
            }

            return mappedUser;
        }



        public async Task<AdminUserDto> AddRoleAsync(int userId, UpdateUserRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserByIdNotFoundException(userId);

            var validRoles = new[] { "Admin", "Doctor", "Patient", "Nurse", "Pharmacist" };
            if (!validRoles.Contains(dto.Role))
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Invalid role." });

            var hasRole = await _userManager.IsInRoleAsync(user, dto.Role);
            if (hasRole)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "User already has this role." });

            var result = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            return await MapToAdminUserDto(user);
        }

        public async Task<AdminUserDto> RemoveRoleAsync(int userId, UpdateUserRoleDto dto, int requestingAdminId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserByIdNotFoundException(userId);

            var hasRole = await _userManager.IsInRoleAsync(user, dto.Role);
            if (!hasRole)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "User does not have this role." });

            if (dto.Role == "Admin" && userId == requestingAdminId)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Admins cannot remove their own Admin role." });

            var result = await _userManager.RemoveFromRoleAsync(user, dto.Role);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            return await MapToAdminUserDto(user);
        }

        public async Task<AdminUserDto> ConvertUserTypeAsync(int userId, ConvertUserTypeDto dto, int requestingAdminId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserByIdNotFoundException(userId);

            if (dto.TargetType != "Doctor" && dto.TargetType != "Nurse" && dto.TargetType != "Pharmacist")
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Target type must be Doctor, Nurse, or Pharmacist." });

            if (user.UserType == dto.TargetType)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "User is already of this type." });

            if (user.UserType == "Patient" || user.UserType == "Admin")
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Cannot convert Patient or Admin accounts." });

            if (dto.TargetType == "Doctor" && string.IsNullOrEmpty(dto.Specialty))
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Specialty is required for Doctor." });
            if (dto.TargetType == "Pharmacist" && string.IsNullOrEmpty(dto.PharmacyName))
                throw new BadRequestException(new System.Collections.Generic.List<string> { "PharmacyName is required for Pharmacist." });

            if (user.UserType == "Doctor")
            {
                var hasActiveAppointments = await _dbContext.Appointments.AnyAsync(a => a.DoctorId == userId && (a.Status == DAL.Shared.Enums.AppointmentStatus.Pending || a.Status == DAL.Shared.Enums.AppointmentStatus.Confirmed));
                if (hasActiveAppointments) throw new ConversionBlockedException("Doctor has active appointments.");
                
                var hasActiveConsultations = await _dbContext.Consultations.AnyAsync(c => c.DoctorId == userId && c.Status == DAL.Shared.Enums.ConsultationStatus.Pending);
                if (hasActiveConsultations) throw new ConversionBlockedException("Doctor has active consultations.");
            }
            else if (user.UserType == "Nurse")
            {
                var hasActiveNursingRequests = await _dbContext.NursingRequests.AnyAsync(n => n.NurseId == userId && n.Status == "Pending");
                if (hasActiveNursingRequests) throw new ConversionBlockedException("Nurse has active nursing requests.");
            }
            else if (user.UserType == "Pharmacist")
            {
                var hasMedications = await _dbContext.Medications.AnyAsync(m => m.PharmacistId == userId);
                if (hasMedications) throw new ConversionBlockedException("Pharmacist owns medications. Reassign or delete them first.");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var sql = @"
                    UPDATE AspNetUsers
                    SET Discriminator = {0},
                        UserType = {0},
                        Specialty = {1},
                        Location = {2},
                        Specialization = {3},
                        PharmacyName = {4}
                    WHERE Id = {5}";
                
                await _dbContext.Database.ExecuteSqlRawAsync(sql, 
                    dto.TargetType,
                    dto.TargetType == "Doctor" ? (object?)dto.Specialty ?? DBNull.Value : DBNull.Value,
                    dto.TargetType == "Doctor" ? (object?)dto.Location ?? DBNull.Value : DBNull.Value,
                    dto.TargetType == "Nurse" ? (object?)dto.Specialization ?? DBNull.Value : DBNull.Value,
                    dto.TargetType == "Pharmacist" ? (object?)dto.PharmacyName ?? DBNull.Value : DBNull.Value,
                    userId);

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, dto.TargetType);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            // We must detach the tracked entity to reload it freshly with the new Discriminator type
            _dbContext.Entry(user).State = EntityState.Detached;

            var updatedUser = await _userManager.FindByIdAsync(userId.ToString());
            if (updatedUser == null)
                throw new UserByIdNotFoundException(userId);

            return await MapToAdminUserDto(updatedUser);
        }

        private async Task EnsureEmailUniqueAsync(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                throw new EmailAlreadyExistsException(email);
        }

        private async Task<AdminUserDto> CreateUserAndAssignRoleAsync(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            return await MapToAdminUserDto(user);
        }

        private async Task<AdminUserDto> MapToAdminUserDto(ApplicationUser user)
        {
            var mappedUser = _mapper.Map<AdminUserDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            mappedUser.Roles = roles.ToList();

            if (user is Doctor doc)
            {
                mappedUser.Specialty = doc.Specialty;
                mappedUser.Location = doc.Location;
            }
            else if (user is Nurse nurse)
            {
                mappedUser.Specialization = nurse.Specialization;
            }
            else if (user is Pharmacist pharm)
            {
                mappedUser.PharmacyName = pharm.PharmacyName;
            }

            return mappedUser;
        }
    }
}
