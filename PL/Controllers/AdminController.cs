using BLL.Dtos.Admin;
using BLL.Services.AbstractServices.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("Doctors")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorAdminDto dto)
        {
            var user = await _adminService.CreateDoctorAsync(dto);
            return StatusCode(201, user);
        }

        [HttpPost("Nurses")]
        public async Task<IActionResult> CreateNurse([FromBody] CreateNurseAdminDto dto)
        {
            var user = await _adminService.CreateNurseAsync(dto);
            return StatusCode(201, user);
        }

        [HttpPost("Pharmacists")]
        public async Task<IActionResult> CreatePharmacist([FromBody] CreatePharmacistAdminDto dto)
        {
            var user = await _adminService.CreatePharmacistAsync(dto);
            return StatusCode(201, user);
        }

        [HttpDelete("Users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var requestingAdminId = User.GetUserId();
            var user = await _adminService.DeleteUserAsync(userId, requestingAdminId);
            return Ok(user);
        }

        [HttpPost("Users/{userId}/Roles/Add")]
        public async Task<IActionResult> AddRole(int userId, [FromBody] UpdateUserRoleDto dto)
        {
            var user = await _adminService.AddRoleAsync(userId, dto);
            return Ok(user);
        }

        [HttpDelete("Users/{userId}/Roles/Remove")]
        public async Task<IActionResult> RemoveRole(int userId, [FromBody] UpdateUserRoleDto dto)
        {
            var requestingAdminId = User.GetUserId();
            var user = await _adminService.RemoveRoleAsync(userId, dto, requestingAdminId);
            return Ok(user);
        }

        [HttpPut("Users/{userId}/Convert")]
        public async Task<IActionResult> ConvertUserType(int userId, [FromBody] ConvertUserTypeDto dto)
        {
            var requestingAdminId = User.GetUserId();
            var user = await _adminService.ConvertUserTypeAsync(userId, dto, requestingAdminId);
            return Ok(user);
        }
    }
}
