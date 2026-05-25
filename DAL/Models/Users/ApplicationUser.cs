using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;

    }
}
