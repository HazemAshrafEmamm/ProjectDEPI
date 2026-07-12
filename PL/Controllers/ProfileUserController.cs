using BLL.Dtos;
using BLL.Services.AbstractServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;

namespace PL.Controllers
{
    [Authorize]
    public class ProfileUserController(IProfileUserService _profileUserService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.GetUserId();
            var profile = await _profileUserService.GetProfileUserByIdAsync(userId);
            
            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMyProfile([FromBody] ProfileUser dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetUserId();
            var updatedProfile = await _profileUserService.UpdateMyProfile(userId, dto);
            
            return Ok(updatedProfile);
        }
    }
}
