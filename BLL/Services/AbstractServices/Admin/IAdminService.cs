using BLL.Dtos.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.Admin
{
    public interface IAdminService
    {
        Task<(IEnumerable<AdminUserDto> Users, int TotalCount)> GetAllUsersAsync(SearchUserDto searchDto);
        Task<AdminUserDto> CreateDoctorAsync(CreateDoctorAdminDto dto);
        Task<AdminUserDto> CreateNurseAsync(CreateNurseAdminDto dto);
        Task<AdminUserDto> CreatePharmacistAsync(CreatePharmacistAdminDto dto);
        Task<AdminUserDto> DeleteUserAsync(int userId, int requestingAdminId);
        Task<AdminUserDto> AddRoleAsync(int userId, UpdateUserRoleDto dto);
        Task<AdminUserDto> RemoveRoleAsync(int userId, UpdateUserRoleDto dto, int requestingAdminId);
    }
}
