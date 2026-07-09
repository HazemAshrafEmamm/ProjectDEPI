using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.Admin
{
    public class ConvertUserTypeDto
    {
        [Required]
        public string TargetType { get; set; } = null!;

        public string? Specialty { get; set; }
        public string? Location { get; set; }
        public string? Specialization { get; set; }
        public string? PharmacyName { get; set; }
    }
}
