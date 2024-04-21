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
            else if (existing.Email != "someemail@gmail.com")
            {
                TempData["Error"] = "You already have account!";
                return View(codeVM);
            }

            TempData["ExistingDoctorId"] = existing.Id;

            return RedirectToAction("Register", "Account");
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            TempData["DoctorId"] = TempData["ExistingDoctorId"];

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);


            var doctorId = TempData["DoctorId"].ToString();
            if (!string.IsNullOrEmpty(doctorId))
            {
                var doctor = await _doctorRepository.GetByIdAsync(doctorId);
                if (doctor != null)
                {
                    _doctorRepository.Delete(doctor);
                    var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
                    if (user != null)
                    {
                        TempData["Error"] = "This email address is already in use";
                        return View(registerVM);
                    }

                    var newUser = new Doctor()
                    {
                        Name = doctor.Name,
                        Surname = doctor.Surname,
                        Title = doctor.Title,
                        Code = doctor.Code,
                        Email = registerVM.EmailAddress,
                        UserName = registerVM.EmailAddress
                    };
                    var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

                    if (newUserResponse.Succeeded)
                        if (newUser.Title == Data.Enum.Title.Nurse)
                            await _userManager.AddToRoleAsync(newUser, UserRole.Nurse);
                        else await _userManager.AddToRoleAsync(newUser, UserRole.Nurse);

                    return RedirectToAction("Index", "Patient");
                    
                }
            }
            return View(registerVM);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
