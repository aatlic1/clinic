using Clinic.Interfaces;
using Clinic.Models;
using Clinic.Repository;
using Clinic.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
                var reception = await _receptionRepository.GetByIdAsyncNoTracking(id);
                if (reception == null) return View("Error");

                var report = new Report()
                {
                    PatientId = reception.PatientId,
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

        public async Task<IActionResult> Detail(int id)
        {
            var report = await _reportRepository.GetReportById(id);
            return View(report);
        }

        public async Task<IActionResult> GeneratePDF(int id)
        {
            var result = await _reportRepository.GetReportById(id);

            var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            document.Add(new Paragraph(result.Caption));
            document.Add(new Paragraph());
            document.Add(new Paragraph("Patient's Name: " + result.Patient.Name + " " + result.Patient.Surname));
            document.Add(new Paragraph("Date of birth: " + result.Patient.BirthDate));
            document.Add(new Paragraph("Gender: " + result.Patient.Gender));
            document.Add(new Paragraph());
            document.Add(new Paragraph("Doctor's Name: " + result.Doctor.Name + " " + result.Doctor.Surname));
            document.Add(new Paragraph("Date: " + result.DateTime));
            document.Add(new Paragraph());
            document.Add(new Paragraph(result.Description));

            document.Close();

            return File(memoryStream.ToArray(), "application/pdf", "MedicalReport.pdf");
        }
    }
}
