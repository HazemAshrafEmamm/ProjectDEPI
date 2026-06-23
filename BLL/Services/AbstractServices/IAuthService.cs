using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using Shared.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstractionLayer
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto RegisterDto);
        Task<bool> CheckEmailAsync (string email);
        Task<UserDto> GetCurrentUserAsync(string email);
        Task<string?> GenerateResetTokenAsync(string email);
        Task<bool> SendResetEmailAsync(string email, string resetLink);
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);


    }
}
