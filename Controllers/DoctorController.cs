using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Clinic.Repository;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<Doctor> _userManager;

        public DoctorController(IDoctorRepository doctorRepository, UserManager<Doctor> userManager)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorRepository.GetAll();
            return View(doctors);
        }
        public async Task<IActionResult> Detail(string id)
        {
            Doctor doctor = await _doctorRepository.GetByIdAsync(id);
            return View(doctor);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorViewModel doctorVM)
        {
            if (ModelState.IsValid)
            {
                var existing = await _doctorRepository.GetByCode(doctorVM.Code);

                if (existing == null)
                {
                    var doctor = new Doctor
                    {
                        Name = doctorVM.Name,
                        Surname = doctorVM.Surname,
                        Title = doctorVM.Title,
                        Code = doctorVM.Code,
                        Email = "someemail@gmail.com",
                        UserName = doctorVM.Name[0] + doctorVM.Surname + doctorVM.Name[0]
                    };

                    var newUserResponse = await _userManager.CreateAsync(doctor, "Jabuka12!");

                    if (newUserResponse.Succeeded)
                    {
                        if (doctor.Title == Data.Enum.Title.Nurse)
                        {
                            await _userManager.AddToRoleAsync(doctor, UserRole.Nurse);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(doctor, UserRole.Doctor);
                        }
                        return RedirectToAction("Index");
                    }
                    return View(doctorVM);

                }

                TempData["Error"] = "Code is already existing. Please try again.";
            }

            return View(doctorVM);

        }
        public async Task<IActionResult> Edit(string id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return View("Error");
            var doctorVM = new EditDoctorViewModel
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Surname = doctor.Surname,
                Title = doctor.Title,
                Code = doctor.Code,
            };
            return View(doctorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditDoctorViewModel doctorVM)
        {
            if (ModelState.IsValid)
            {
                var detail = await _doctorRepository.GetByIdAsyncNoTracking(id);

                if (detail == null)
                {
                    return View("Error");
                }

                if (detail.Code != doctorVM.Code)
                {
                    var existing = await _doctorRepository.GetByCode(doctorVM.Code);

                    if(existing != null)
                    {
                        TempData["Error"] = "Code is already existing. Please try again.";
                        return View(doctorVM);

                    }
                }
                detail.Id = doctorVM.Id;
                detail.Name = doctorVM.Name;
                detail.Surname = doctorVM.Surname;
                detail.Title = doctorVM.Title;
                detail.Code = doctorVM.Code;

                _doctorRepository.Update(detail);

                return RedirectToAction("Index");

            }
            return View(doctorVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var doctor = await _doctorRepository.GetByIdAsyncNoTracking(id);
            if (doctor == null) return View("Error");

            _doctorRepository.Delete(doctor);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Search(string searchDoctor)
        {
            if (string.IsNullOrEmpty(searchDoctor))
            {
                return RedirectToAction("Index");
            }

            var doctors = await _doctorRepository.GetDoctorByNameOrCode(searchDoctor);
            return View("Index", doctors);
        }
    }
}
