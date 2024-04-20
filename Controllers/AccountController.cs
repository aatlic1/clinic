using Clinic.Data;
using Clinic.Models;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Doctor> _userManager;
        private readonly SignInManager<Doctor> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<Doctor> userManager,
            SignInManager<Doctor> signInManager,
            ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Reception");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            else
            {
                TempData["Error"] = "Wrong credentials. Please try again";
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Reception");
        }
    }
}
