using DAL.Models;

namespace BLL.Dtos.User;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}