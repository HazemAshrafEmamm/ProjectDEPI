using DAL.Models;

namespace BLL.Dtos.User;

public class UpdateUserDto
{
    public string Name { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}