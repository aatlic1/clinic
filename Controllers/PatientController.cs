using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Clinic.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IReportRepository _reportRepository;

        public PatientController(IPatientRepository patientRepository, IReportRepository reportRepository)
        {
            _patientRepository = patientRepository;
            _reportRepository = reportRepository;
        }
        public async Task<IActionResult> Index()
        {
            var patients = await _patientRepository.GetAll();
            return View(patients);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Patient patient = await _patientRepository.GetByIdAsync(id);
            var reports = await _reportRepository.GetReportsByPatientId(id);
            var patientVM = new PatientViewModel()
            {
                Id = patient.Id,
                Name = patient.Name,
                Surname = patient.Surname,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                AddressId = patient.AddressId,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                Reports = reports
            };
            return View(patientVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                return View(patientVM);
            }

            var patient = new Patient
            {
                Id = patientVM.Id,
                Name = patientVM.Name,
                Surname = patientVM.Surname,
                BirthDate = patientVM.BirthDate,
                Gender = patientVM.Gender,
                AddressId = patientVM.AddressId,
                Address = new Address
                {
                    Street = patientVM.Address.Street,
                    City = patientVM.Address.City,
                    State = patientVM.Address.State,
                },
                PhoneNumber = patientVM.PhoneNumber
            };

            _patientRepository.Add(patient);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return View("Error");
            var patientVM = new EditPatientViewModel
            {
                Id = patient.Id,
                Name = patient.Name,
                Surname = patient.Surname,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                AddressId = patient.AddressId,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber
            };
            return View(patientVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditPatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit patient");
                return View(patientVM);
            }

            var detail = await _patientRepository.GetByIdAsyncNoTracking(id);

            if (detail == null)
            {
                return View("Error");
            }

            var patient = new Patient
            {
                Id = patientVM.Id,
                Name = patientVM.Name,
                Surname = patientVM.Surname,
                BirthDate = patientVM.BirthDate,
                Gender = patientVM.Gender,
                AddressId = patientVM.AddressId,
                Address = patientVM.Address,
                PhoneNumber = patientVM.PhoneNumber
            };

            _patientRepository.Update(patient);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientRepository.GetByIdAsyncNoTracking(id);
            if (patient == null) return View("Error");

            _patientRepository.Delete(patient);
            return RedirectToAction("Index");
        }
    }
}
