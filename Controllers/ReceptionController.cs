using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Clinic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class ReceptionController : Controller
    {
        private readonly IReceptionRepository _receptionRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public ReceptionController(IReceptionRepository receptionRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _receptionRepository = receptionRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }
        public async Task<IActionResult> Index()
        {
            var receptions = await _receptionRepository.GetAll();
            return View(receptions);
        }
        public async Task<IActionResult> Create()
        {
            var patients = await _patientRepository.GetAll();
            ViewBag.PatientList = new SelectList(patients.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} {p.Surname}"
            }), "Value", "Text");

            var doctors = await _doctorRepository.GetSpecialistAndResident();
            ViewBag.DoctorList = new SelectList(doctors.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Name} {d.Surname} - {d.Code}"
            }), "Value", "Text");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reception reception)
        {
            if (!ModelState.IsValid)
            {
                return View(reception);
            }
            _receptionRepository.Add(reception);
            return RedirectToAction("Index");
        }
    }
}
