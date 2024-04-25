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
    public class PatientControllerTests
    {
        private IPatientRepository _patientRepository;
        private IReportRepository _reportRepository;
        private PatientController _patientController;

        public PatientControllerTests() 
        {
            _patientRepository = A.Fake<IPatientRepository>();
            _reportRepository = A.Fake<IReportRepository>();

            _patientController = new PatientController(_patientRepository, _reportRepository);
        }

        [Fact]
        public void PatientController_Index_ReturnsSuccess()
        {
            var patients = A.Fake<IEnumerable<Patient>>();
            A.CallTo(() => _patientRepository.GetAll()).Returns(patients);

            var result = _patientController.Index();

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PatientController_Detail_ReturnsSuccess()
        {
            var id = 1;
            var patient = A.Fake<Patient>();
            A.CallTo(() => _patientRepository.GetByIdAsync(id)).Returns(patient);

            var result = _patientController.Detail(id);

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public async Task PatientController_Create_ReturnsRedirectToIndex()
        {
            var patientViewModel = new CreatePatientViewModel
            {
                Name = "Max",
                Surname = "Williams",
                BirthDate = new DateOnly(1987, 3, 3),
                Gender = Gender.Male,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                },
                PhoneNumber = "123-456-7890"
            };

            var result = await _patientController.Create(patientViewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task PatientController_Edit_ReturnsRedirectToIndex()
        {
            var editPatientViewModel = new EditPatientViewModel
            {
                Id = 1,
                Name = "Max",
                Surname = "Williams",
                BirthDate = new DateOnly(1987, 3, 3),
                Gender = Gender.Male,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                }
            };

            var result = await _patientController.Edit(1, editPatientViewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task PatientController_Delete_ReturnsRedirectToIndex()
        {
            var result = await _patientController.Delete(1) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }
        [Fact]
        public void PatientController_Search_ReturnsSuccess()
        {
            var patients = A.Fake<IEnumerable<Patient>>();
            A.CallTo(() => _patientRepository.GetPatientByName("Ma")).Returns(patients);

            var result = _patientController.Search("Ma");

            result.Should().BeOfType<Task<IActionResult>>();
        }

    }
}
