using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Magistri.Models;

public class Subject { 
    public int Id {get; set;}
    [NotNull]
    public String Name {get; set;}
}