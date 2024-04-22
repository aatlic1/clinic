using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Clinic.Repository;
using Clinic.ViewModels;
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

        public ReceptionController(IReceptionRepository receptionRepository, IPatientRepository patientRepository, 
            IDoctorRepository doctorRepository)
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
        public async Task<IActionResult> Create(CreateReceptionViewModel receptionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(receptionVM);
            }

            var reception = new Reception 
            { 
                Id = receptionVM.Id,
                PatientId = receptionVM.PatientId,
                Patient = receptionVM.Patient,
                DoctorId = receptionVM.DoctorId,
                Doctor = receptionVM.Doctor,
                DateTime = receptionVM.DateTime,
                Emergency = receptionVM.Emergency
            };

            _receptionRepository.Add(reception);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
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

            var reception = await _receptionRepository.GetByIdAsyncNoTracking(id);
            if (reception == null) return View("Error");
            var receptionVM = new EditReceptionViewModel
            {
                Id = reception.Id,
                PatientId = reception.PatientId,
                Patient = reception.Patient,
                DoctorId = reception.DoctorId,
                Doctor = reception.Doctor,
                DateTime = reception.DateTime,
                Emergency = reception.Emergency
            };
            return View(receptionVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditReceptionViewModel receptionVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit reception");
                return View(receptionVM);
            }

            var detail = await _receptionRepository.GetByIdAsyncNoTracking(id);

            if (detail == null)
            {
                return View("Error");
            }

            var reception = new Reception
            {
                Id = receptionVM.Id,
                PatientId = receptionVM.PatientId,
                Patient = receptionVM.Patient,
                DoctorId = receptionVM.DoctorId,
                Doctor = receptionVM.Doctor,
                DateTime = receptionVM.DateTime,
                Emergency = receptionVM.Emergency
            };

            _receptionRepository.Update(reception);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var reception = await _receptionRepository.GetByIdAsyncNoTracking(id);
            if (reception == null) return View("Error");

            _receptionRepository.Delete(reception);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Filter(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                var receptions = await _receptionRepository.GetAll();
                return PartialView("_ReceptionsTable", receptions);
            }
            else
            {
                var receptions = await _receptionRepository.GetReceptionsByDates(startDate.Value, endDate.Value);
                return PartialView("_ReceptionsTable", receptions);
            }
        }
    }
}
