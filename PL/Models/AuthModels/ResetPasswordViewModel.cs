using System.ComponentModel.DataAnnotations;

namespace PL.Models.IdentityViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Enter ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
