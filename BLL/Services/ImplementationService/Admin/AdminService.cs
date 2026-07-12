using AutoMapper;
using BLL.Dtos.Admin;
using BLL.Services.AbstractServices.Admin;
using DAL.Exceptions;
using DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ImplementationService.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AdminService(UserManager<ApplicationUser> userManager, IMapper mapper, IUserRepository userRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<(IEnumerable<AdminUserDto> Users, int TotalCount)> GetAllUsersAsync(SearchUserDto searchDto)
        {
            var (users, totalCount) = await _userRepository.SearchUsersAsync(
                searchDto.Name,
                searchDto.Email,
                searchDto.UserType,
                searchDto.Role,
                searchDto.IsActive,
                searchDto.PageNumber,
                searchDto.PageSize);

            var mappedUsers = new List<AdminUserDto>();
            foreach (var user in users)
            {
                mappedUsers.Add(await MapToAdminUserDto(user));
            }

            return (mappedUsers, totalCount);
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

            if (userId == requestingAdminId)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Admins cannot delete themselves." });

            var isTargetAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isTargetAdmin)
                throw new BadRequestException(new System.Collections.Generic.List<string> { "Cannot delete another Admin account." });

            var mappedUser = await MapToAdminUserDto(user);
            
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
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
                throw new BadRequestException(new List<string> { "Invalid role." });

            var hasRole = await _userManager.IsInRoleAsync(user, dto.Role);
            if (hasRole)
                throw new BadRequestException(new List<string> { "User already has this role." });

            bool requiresTypeConversion = (dto.Role == "Doctor" || dto.Role == "Nurse" || dto.Role == "Pharmacist") && user.UserType != dto.Role;

            if (requiresTypeConversion)
            {
                if (user.UserType == "Patient" || user.UserType == "Admin")
                    throw new BadRequestException(new List<string> { "Cannot convert Patient or Admin accounts to another type." });

                if (dto.Role == "Doctor" && string.IsNullOrEmpty(dto.Specialty))
                    throw new BadRequestException(new List<string> { "Specialty is required for Doctor." });
                if (dto.Role == "Pharmacist" && string.IsNullOrEmpty(dto.PharmacyName))
                    throw new BadRequestException(new List<string> { "PharmacyName is required for Pharmacist." });

                if (user.UserType == "Doctor")
                {
                    var hasActiveAppointments = await _userRepository.HasActiveAppointmentsAsync(userId);
                    if (hasActiveAppointments) throw new ConversionBlockedException("Doctor has active appointments.");
                    
                    var hasActiveConsultations = await _userRepository.HasActiveConsultationsAsync(userId);
                    if (hasActiveConsultations) throw new ConversionBlockedException("Doctor has active consultations.");
                }
                else if (user.UserType == "Nurse")
                {
                    var hasActiveNursingRequests = await _userRepository.HasActiveNursingRequestsAsync(userId);
                    if (hasActiveNursingRequests) throw new ConversionBlockedException("Nurse has active nursing requests.");
                }
                else if (user.UserType == "Pharmacist")
                {
                    var hasMedications = await _userRepository.HasMedicationsAsync(userId);
                    if (hasMedications) throw new ConversionBlockedException("Pharmacist owns medications. Reassign or delete them first.");
                }

                if (validRoles.Contains(user.UserType))
                {
                     await _userManager.RemoveFromRoleAsync(user, user.UserType);
                }

                await _userRepository.ConvertUserTypeAsync(userId, dto.Role, dto.Specialty, dto.Location, dto.Specialization, dto.PharmacyName);
            }

            var result = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            var updatedUser = await _userManager.FindByIdAsync(userId.ToString());
            if (updatedUser == null)
                throw new UserByIdNotFoundException(userId);

            return await MapToAdminUserDto(updatedUser);
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
