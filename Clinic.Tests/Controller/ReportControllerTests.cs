using Clinic.Controllers;
using Clinic.Interfaces;
using Clinic.Models;
using Clinic.Repository;
using Clinic.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Tests.Controller
{
    public class ReportControllerTests
    {
        private IReceptionRepository _receptionRepository;
        private IReportRepository _reportRepository;
        private ReportController _reportController;

        public ReportControllerTests()
        {
            _receptionRepository = A.Fake<IReceptionRepository>();
            _reportRepository = A.Fake<IReportRepository>();

            _reportController = new ReportController(_reportRepository, _receptionRepository);
        }
        [Fact]
        public void ReportController_Detail_ReturnsSuccess()
        {
            var report = A.Fake<Report>();
            A.CallTo(() => _reportRepository.GetReportById(1)).Returns(report);

            var result = _reportController.Detail(1);

            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public async void ReportController_Create_ReturnsRedirectToIndex()
        {
            var reportViewModel = new CreateReportViewModel
            {
                PatientId = 1,
                DoctorId = "63ca35da-f5b2-45be-928a-7669af06f82c",
                DateTime = new DateTime(new DateOnly(2024, 6, 5), new TimeOnly(12, 10)),
                Caption = "REPORT",
                Description = "Description"
            };

            var result = await _reportController.Create(1, reportViewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }
        [Fact]
        public void ReportController_GeneratePDF_ReturnSuccess()
        {
            var report = A.Fake<Report>();
            A.CallTo(() => _reportRepository.GetReportById(1)).Returns(report);

            var result = _reportController.GeneratePDF(1);

            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
