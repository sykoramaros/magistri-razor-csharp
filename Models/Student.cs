using System.ComponentModel.DataAnnotations.Schema;

namespace Magistri.Models {
    public class Student {
        public int Id { get; set; }
        public String FirstName { get; set; } = String.Empty;
        public String LastName { get; set; } = String.Empty;
        public DateOnly DateOfBirth { get; set; }
        [NotMapped]
        public String FullName { get=> $"{FirstName} {LastName}";  }
    }
}
