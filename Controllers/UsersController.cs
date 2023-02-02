using Microsoft.AspNetCore.Mvc;
using todo.Areas.Identity.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace todo.Controllers;

public class UsersController : Controller
{

    private readonly AppDbContext db;
    private SignInManager<IdentityUser> _signManager; 
    private UserManager<IdentityUser> _userManager;  
    public UsersController(AppDbContext appDb, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signManager)
    {
        db = appDb;
        _userManager = userManager; 
        _signManager = signManager; 
    }
    [HttpGet]
    [Route("/login")]
    public IActionResult Login()
    {
        if(HttpContext.User?.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Tasks");
        LoginViewModel viewModel = new LoginViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [Route("/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel data)
    {
        var result = await _signManager.PasswordSignInAsync(data.email, data.password, true, true);
        if(!result.Succeeded)
        {
            ModelState.AddModelError("","Invalid login attempt"); 
            return View(data); 
        }
        return RedirectToAction("Index", "Tasks");
    }

}