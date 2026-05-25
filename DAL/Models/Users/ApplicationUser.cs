using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; } = null!;
        public bool IsActive { get; set; } = true;

    }
}
