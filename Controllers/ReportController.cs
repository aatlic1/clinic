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
                    DateTime = reception.DateTime,
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

            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Paragraph caption = new Paragraph(result.Caption, boldFont);
            caption.Alignment = Element.ALIGN_CENTER;
            document.Add(caption);

            document.Add(new Paragraph(" "));

            PdfPTable patientTable = new PdfPTable(2);
            patientTable.AddCell("Patient's Name:");
            patientTable.AddCell(result.Patient.Name + " " + result.Patient.Surname);
            patientTable.AddCell("Date of birth:");
            patientTable.AddCell(result.Patient.BirthDate.ToString());
            patientTable.AddCell("Gender:");
            patientTable.AddCell(result.Patient.Gender.ToString());
            document.Add(patientTable);

            document.Add(new Paragraph(" "));

            PdfPTable doctorTable = new PdfPTable(2);
            doctorTable.AddCell("Doctor's Name:");
            doctorTable.AddCell(result.Doctor.Name + " " + result.Doctor.Surname);
            doctorTable.AddCell("Date:");
            doctorTable.AddCell(result.DateTime.ToString());
            document.Add(doctorTable);

            document.Add(new Paragraph(" "));

            document.Add(new Paragraph(result.Description));

            document.Close();

            return File(memoryStream.ToArray(), "application/pdf", "MedicalReport.pdf");
        }
    }
}
