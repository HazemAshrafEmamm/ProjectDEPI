using System.ComponentModel.DataAnnotations;

namespace PL.Models.IdentityViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Cant be Empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
