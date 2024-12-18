using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Magistri.DTO;

public class SubjectDto {
    public int Id { get; set; }
    [MaxLength(25)]
    [MinLength(4, ErrorMessage = "The subject name is too short, the minimum length is 4")]
    [DisplayName("Subject name")]
    public String Name { get; set; }
}