using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Clinic.Repository;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorRepository.GetAll();
            return View(doctors);
        }
        public async Task<IActionResult> Detail(int id)
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
            if (!ModelState.IsValid)
            {
                return View(doctorVM);
            }

            var doctor = new Doctor
            {
                Id = doctorVM.Id,
                Name = doctorVM.Name,
                Surname = doctorVM.Surname,
                Title = doctorVM.Title,
                Code = doctorVM.Code,
            };

            _doctorRepository.Add(doctor);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(int id, EditDoctorViewModel doctorVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit doctor");
                return View(doctorVM);
            }

            var detail = await _doctorRepository.GetByIdAsyncNoTracking(id);

            if (detail == null)
            {
                return View("Error");
            }

            var doctor = new Doctor
            {
                Id = doctorVM.Id,
                Name = doctorVM.Name,
                Surname = doctorVM.Surname,
                Title = doctorVM.Title,
                Code = doctorVM.Code,
            };

            _doctorRepository.Update(doctor);

            return RedirectToAction("Index");
        }
    }
}
