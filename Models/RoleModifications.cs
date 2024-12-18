namespace Magistri.Models;
// vystupni model
// seznam pridanych Id a jejich role
// reseni pro checkboxy
public class RoleModifications {
    public string RoleName { get; set; }
    public string RoleId { get; set; }  // pro preneseni Id do backendu
    public string[]? AddIds { get; set; }
    public string[]? DeleteIds { get; set; }
}