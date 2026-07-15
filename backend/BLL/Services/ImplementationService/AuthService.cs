using AutoMapper;
using Microsoft.Extensions.Configuration;
using DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using Shared.DTOs.IdentityDtos;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ServiceAbstractionLayer;
using PL.Utilites;
using DAL.Exceptions;

namespace BLL.Services.ImplementationService
{
    public class AuthService(UserManager<ApplicationUser> _userManager,
                            IConfiguration _configuration) : IAuthService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }
        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                            ?? throw new UserNotFoundException(email);

            return new UserDto()
            {
                Email = user.Email!,
                FullName = user.Fullname,
                Token = await GenerateJwtToken(user)
            };
        }
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email)
                ?? throw new UserNotFoundException(loginDto.Email);

            var passwordValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);

            if (!passwordValid)
                throw new InvalidCredentialsException();

            return new UserDto
            {
                Email = User.Email!,
                FullName = User.Fullname,
                Token = await GenerateJwtToken(User)
            };

        }
        public async Task<UserDto> RegisterAsync(RegisterDto RegisterDto)
        {
            var user = new Patient
            {
                Fullname = RegisterDto.DisplayName,
                Email = RegisterDto.Email,
                UserName = RegisterDto.UserName ?? RegisterDto.Email.Split("@")[0],
                PhoneNumber = RegisterDto.PhoneNumber,
                UserType = "Patient",
            };
            
            var result = await _userManager.CreateAsync(user, RegisterDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user,"PATIENT");
                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors.Select(e => e.Description).ToList();
                    throw new BadRequestException(errors);
                }
                return new UserDto
                {
                    Email = user.Email!,
                    FullName = user.Fullname,
                    Token = await GenerateJwtToken(user)
                };  
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
        }

        public async Task<string?> GenerateResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return null;

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<bool> SendResetEmailAsync(string email, string resetLink)
        {
            var mail = new Email()
            {
                To = email,
                Subject = "Reset Password",
                Body = resetLink
            };
            return await EmailSettings.SendEmail(mail);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid Request" });

            var decodedToken = Uri.UnescapeDataString(token);
            return await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
        }


        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()!)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var SecretKey = _configuration["JwtOptions:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));

            var Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Credentials
            );

            var TokenHandler = new JwtSecurityTokenHandler().WriteToken(Token);

            return TokenHandler;
        }


    }
}

