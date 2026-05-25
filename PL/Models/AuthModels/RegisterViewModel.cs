using System.ComponentModel.DataAnnotations;

namespace PL.Models.IdentityViewModels
{
    public class RegisterViewModel
    {


        [Required(ErrorMessage = "Name Can Not Be Null")]
        [Display(Name = "Full Name")]
        [MaxLength(50)]
        public string Fullname { get; set; } = null!;

        [Required(ErrorMessage = "User Name Can Not Be Null")]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
