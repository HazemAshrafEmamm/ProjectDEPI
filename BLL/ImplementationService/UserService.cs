using BLL.AbstractServices;
using BLL.Dtos.User;
using DAL.Models;
using DAL.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.ImplementationService;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository repository,
        IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var existingUser = await _repository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new Exception("Email already exists");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Role = dto.Role,
            IsActive = true
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<bool> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return false;

        user.Name = dto.Name;
        user.Role = dto.Role;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        _repository.Update(user);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return false;

        _repository.Delete(user);
        await _repository.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> ValidatePasswordAsync(string email, string password)
    {
        var user = await _repository.GetByEmailAsync(email);
        if (user == null) return false;

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password
        );

        return result == PasswordVerificationResult.Success;
    }

    // --- Helpers ---

    private string HashPassword(string password)
    {
        // Replace with a real hashing algorithm like BCrypt
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive
        };
    }
}