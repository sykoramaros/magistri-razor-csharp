using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// tato trida resi ktery uzivatel je logovany z databaze loginu

namespace Magistri.Controllers;
[Authorize(Roles = "Admin")]
public class RolesController : Controller {
    private RoleManager<IdentityRole> roleManager;
    private UserManager<AppUser> userManager;
    // GET

    public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) {
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    public IActionResult Index() {
        return View(roleManager.Roles);
    }

    public IActionResult Create() => View();    // zkracena verze zapisu klasickeho

    [HttpPost]
    public async Task<IActionResult> CreateAsync(string name) {
        IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
        if (result.Succeeded) {
            return RedirectToAction("Index");
        } else {
            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            };
            return View(name);
        }
    }
    
    // mazání role na základě jejího ID.
    [HttpPost]
    public async Task<IActionResult> DeleteAsync(string id) {
        IdentityRole foundRole = await roleManager.FindByIdAsync(id);
        if (foundRole != null) {
            IdentityResult delete = await roleManager.DeleteAsync(foundRole);
            if (delete.Succeeded) {
                return RedirectToAction("Index");
            } else {
                foreach (var error in delete.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        } else {
            ModelState.AddModelError(string.Empty, "Role not found");
        }
        return RedirectToAction(nameof(Index));     // varianta ("Index")
    }

    
    public async Task<IActionResult> EditAsync(string id) {
        IdentityRole roleToEdit = await roleManager.FindByIdAsync(id);
        List<AppUser> members = new List<AppUser>();
        List<AppUser> nonMembers = new List<AppUser>();
        if (roleToEdit != null) {       // pokud se role najde je potreba otestovat jestli uzivatel v roli je a podle toho se priradi do prislusneho listu
            foreach (AppUser user in userManager.Users) {
                var list = await userManager.IsInRoleAsync(user, roleToEdit.Name) ? members :
                nonMembers; // Name ktery se vytahne z databaze kde jsou uzivatele ulozeni v prirazeni nejake roli a to se checkuje
                list.Add(user);
            }
            return View(new RoleEdit {
                Role = roleToEdit,
                Members = members,
                NonMembers = nonMembers
            });
        } else {
            ModelState.AddModelError(string.Empty, "Role not found");
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditAsync(RoleModifications modification) {
        foreach (string userID in modification.AddIds ?? Array.Empty<string>()) {   // varianta [] collection expression
            AppUser user = await userManager.FindByIdAsync(userID);
            if (user != null) {
                IdentityResult result = await userManager.AddToRoleAsync(user, modification.RoleName);
                if (!result.Succeeded) {
                    AddModelErrors(result);
                }
            }
        }

        foreach (string userId in modification.DeleteIds ?? Array.Empty<string>()) {
            AppUser user = await userManager.FindByIdAsync(userId);
            if (user != null) {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, modification.RoleName);
                if (!result.Succeeded) {
                    AddModelErrors(result);
                }
            }
        }
        return RedirectToAction("Index");
    }
    private void AddModelErrors(IdentityResult result) {
        foreach (var error in result.Errors) {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}