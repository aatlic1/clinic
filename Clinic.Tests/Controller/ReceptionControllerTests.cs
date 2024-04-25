using Clinic.Controllers;
using Clinic.Data.Enum;
using Clinic.Interfaces;
using Clinic.Models;
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
    public class ReceptionControllerTests
    {
        private IReceptionRepository _receptionRepository;
        private IDoctorRepository _doctorRepository;
        private IReportRepository _reportRepository;
        private IPatientRepository _patientRepository;
        private ReceptionController _receptionController;

        public ReceptionControllerTests()
        {
            _receptionRepository = A.Fake<IReceptionRepository>();
            _doctorRepository = A.Fake<IDoctorRepository>();
            _reportRepository = A.Fake<IReportRepository>();
            _patientRepository = A.Fake<IPatientRepository>();

            _receptionController = new ReceptionController(_receptionRepository, _patientRepository, _doctorRepository, _reportRepository);
        }

        [Fact]
        public void ReceptionController_Index_ReturnsSuccess()
        {
            var receptions = A.Fake<IEnumerable<Reception>>();
            A.CallTo(() => _receptionRepository.GetAll()).Returns(receptions);

            var result = _receptionController.Index();

            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public async Task ReceptionController_Create_ReturnsRedirectToIndex()
        {
            var receptionViewModel = new CreateReceptionViewModel
            {
                PatientId = 1,
                DoctorId = "63ca35da-f5b2-45be-928a-7669af06f82c",
                DateTime = new DateTime(new DateOnly(2024, 6, 5), new TimeOnly(12, 10)),
                Emergency = false
            };

            var result = await _receptionController.Create(receptionViewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task ReceptionController_Edit_ReturnsRedirectToIndex()
        {
            var receptionViewModel = new EditReceptionViewModel
            {
                PatientId = 1,
                DoctorId = "63ca35da-f5b2-45be-928a-7669af06f82c",
                DateTime = new DateTime(new DateOnly(2024, 6, 5), new TimeOnly(12, 10)),
                Emergency = false
            };

            var result = await _receptionController.Edit(1, receptionViewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task ReceptionController_Delete_ReturnsRedirectToIndex()
        {

            var result = await _receptionController.Delete(1) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }
        [Fact]
        public void ReceptionController_Filter_ReturnSuccess()
        {
            var receptions = A.Fake<IEnumerable<Reception>>();
            var startDate = new DateTime(2024, 4, 4);
            var endDate = new DateTime(2024, 6, 3);
            A.CallTo(() => _receptionRepository.GetReceptionsByDates(startDate, endDate)).Returns(receptions);

            var result = _receptionController.Filter(startDate, endDate);

            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public void ReceptionController_GeneratePDF_ReturnSuccess()
        {
            var reception = A.Fake<Reception>();
            A.CallTo(() => _receptionRepository.GetByIdAsync(1)).Returns(reception);
            var report = A.Fake<Report>();
            A.CallTo(() => _reportRepository.GetReportByReception(reception)).Returns(report);

            var result = _receptionController.GeneratePDF(1);

            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
