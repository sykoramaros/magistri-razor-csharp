using Microsoft.AspNetCore.Identity;
// vstupni model
// nova trida kvuli tomu ze se v jednom okamziku musi resit memberi a nememberi
namespace Magistri.Models;
// pro zobrazeni cleny a necleny
public class RoleEdit {
    public IdentityRole Role { get; set; }
    public IEnumerable<AppUser> Members { get; set; }
    public IEnumerable<AppUser> NonMembers { get; set; }
}