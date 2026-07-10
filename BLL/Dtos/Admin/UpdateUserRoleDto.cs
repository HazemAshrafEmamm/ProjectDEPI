using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.Admin
{
    public class UpdateUserRoleDto
    {
        [Required]
        public string Role { get; set; } = null!;
    }
}
