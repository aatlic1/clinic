using Clinic.Interfaces;
using Clinic.Models;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReceptionRepository _receptionRepository;

        public ReportController(IReportRepository reportRepository, IReceptionRepository receptionRepository)
        {
            _reportRepository = reportRepository;
            _receptionRepository = receptionRepository;
        }
        public async Task<IActionResult> Create(int id)
        {
            var reception = await _receptionRepository.GetByIdAsync(id);
            if (reception == null) return View("Error");
            var reportVM = new CreateReportViewModel()
            {
                PatientId = reception.PatientId,
                Patient = reception.Patient,
                DoctorId = reception.DoctorId,
                Doctor = reception.Doctor,
                DateTime = reception.DateTime
            };
            return View(reportVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateReportViewModel reportVM)
        {
            if (ModelState.IsValid)
            {
                var reception = await _receptionRepository.GetByIdAsync(id);
                if (reception == null) return View("Error");

                var report = new Report()
                {
                    PatientId = reception.PatientId, // Handle nullable PatientId
                    Patient = reportVM.Patient,
                    DoctorId = reception.DoctorId,
                    Doctor = reportVM.Doctor,
                    DateTime = reportVM.DateTime,
                    Caption = reportVM.Caption,
                    Description = reportVM.Description
                };

                _reportRepository.Add(report);

                return RedirectToAction("Index", "Reception");
            }

            return View(reportVM);
        }
    }
}
