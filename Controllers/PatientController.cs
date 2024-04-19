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
    }
}
