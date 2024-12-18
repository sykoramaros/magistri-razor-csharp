using System.ComponentModel.DataAnnotations;

namespace Magistri.DTO {
    public class StudentDto {
        public int Id { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;
        [Display(Name = "Date of birth")]
        public DateOnly DateOfBirth { get; set; }
    }
}
