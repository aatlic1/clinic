using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
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
    }
}
