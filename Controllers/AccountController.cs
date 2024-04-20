using Clinic.Data;
using Clinic.Interfaces;
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
        private readonly IDoctorRepository _doctorRepository;

        public AccountController(UserManager<Doctor> userManager, SignInManager<Doctor> signInManager,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
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
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Reception");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginVM);
            }
            else
            {
                TempData["Error"] = "Wrong credentials. Please try again";
            }
            return View(loginVM);
        }

        public async Task<IActionResult> Code()
        {
            var response = new CodeViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Code(CodeViewModel codeVM)
        {
            var existing = await _doctorRepository.GetByCode(codeVM.Code);

            if(existing == null)
            {
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(codeVM);
            }

            return RedirectToAction("Register", "Account", new { existingDoctorId = existing.Id });
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM, string doctorId)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            await _userManager.SetEmailAsync(doctor, registerVM.EmailAddress);

            // Generisanje tokena za potvrdu novog emaila (opciono)
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(doctor);

            // Potvrda novog emaila (opciono)
            await _userManager.ConfirmEmailAsync(doctor, token);

            // Promena lozinke korisnika
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(doctor);
            var resetResult = await _userManager.ResetPasswordAsync(doctor, resetToken, registerVM.Password);

            if (!resetResult.Succeeded)
            {
                // Ako promena lozinke nije uspela, prikaži odgovarajuću poruku o grešci
                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }


            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Reception");
        }
    }
}
