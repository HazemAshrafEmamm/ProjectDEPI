
using BLL.Dtos.IdentityDtos;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Controller;
using ServiceAbstractionLayer;
using Shared.DTOs.IdentityDtos;

namespace PL.Controllers
{
    public class AuthController(IAuthService _authenticationService ) : ApiControllerBase
    {
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _authenticationService.LoginAsync(loginDto);
            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = await _authenticationService.RegisterAsync(model);
            return Ok(user);
        }

        #region ForgetPassword

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authenticationService.GenerateResetTokenAsync(dto.Email);
            if (token is null)
                return BadRequest(new { message = "Invalid Email Address" });

            var resetLink = Url.Action("ResetPassword", "Auth",
                new { email = dto.Email,token }, Request.Scheme);

            var sent = await _authenticationService.SendResetEmailAsync(dto.Email, resetLink!);
            if (!sent)
                return StatusCode(500, new { message = "Failed to send email" });

            return Ok(new { message = "Reset link sent, please check your inbox." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Password reset successfully." });
        }
        #endregion


    }
}
