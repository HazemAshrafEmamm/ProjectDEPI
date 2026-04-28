using DAL.Shared;

namespace DAL.Models;

public enum UserRole
{
    Admin,
    User,
    Manager
}

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
}