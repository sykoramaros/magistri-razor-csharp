using System.ComponentModel.DataAnnotations.Schema;

namespace Magistri.ViewModels;

public class GradesVM {
    public int Id { get; set; }
    public string StudentName { get; set; }
    
    public string StudentLastName { get; set; }
    public string SubjectName { get; set; }
    public string Topic { get; set; }
    public int Mark { get; set; }
    public DateTime Date { get; set; }
}