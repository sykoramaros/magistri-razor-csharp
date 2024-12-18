using Magistri.DTO;
using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
 
namespace Magistri.Controllers;
[Authorize] // ochrana pred nepovolanymi navstevniky
public class AccountController : Controller {  
    private UserManager<AppUser> userManager;
    private SignInManager<AppUser> signInManager;
    // GET
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {  // konstruktor
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl) {
        LoginDto loginDto = new LoginDto();
        loginDto.ReturnUrl = returnUrl;
        return View(loginDto);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto) {
        if (ModelState.IsValid) {
            AppUser appUser = await userManager.FindByNameAsync(loginDto.UserName);
            if (appUser != null) {
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(appUser, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
                if (signInResult.Succeeded) {
                    return Redirect(loginDto.ReturnUrl ?? "/");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login - check username and password.");
        }
        return View(loginDto);
    }

    public async Task<IActionResult> Logout() {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() {
        return View("AccessDenied");
    }
}