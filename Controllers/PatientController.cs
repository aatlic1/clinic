using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<IActionResult> Index()
        {
            var patients = await _patientRepository.GetAll();
            return View(patients);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Patient patient = await _patientRepository.GetByIdAsync(id); 
            return View(patient);
        }
    }
}
