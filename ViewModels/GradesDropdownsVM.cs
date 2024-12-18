using Magistri.Models;

namespace Magistri.ViewModels;

public class GradesDropdownsVM {

    public List<Student> Students { get; set; }
    public List<Subject> Subjects { get; set; }
    
    public GradesDropdownsVM() {
        Students = new List<Student>();
        Subjects = new List<Subject>();
    }
}