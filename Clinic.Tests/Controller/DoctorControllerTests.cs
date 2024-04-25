using Clinic.Controllers;
using Clinic.Data.Enum;
using Clinic.Interfaces;
using Clinic.Models;
using Clinic.Repository;
using Clinic.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Tests.Controller
{
    public class DoctorControllerTests
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<Doctor> _userManager;
        private readonly DoctorController _doctorController;

        public DoctorControllerTests()
        {
            _doctorRepository = A.Fake<IDoctorRepository>();
            _userManager = A.Fake<UserManager<Doctor>>();

            _doctorController = new DoctorController(_doctorRepository, _userManager);
        }
        [Fact]
        public void DoctorController_Index_ReturnsSuccess()
        {
            var doctors = A.Fake<IEnumerable<Doctor>>();
            A.CallTo(() => _doctorRepository.GetAll()).Returns(doctors);

            var result = _doctorController.Index();

            result.Should().BeOfType<Task<IActionResult>>();

        }
        [Fact]
        public void DoctorController_Detail_ReturnsSuccess()
        {
            var id = "18bd07f5-f1d8-4c72-9bcd-8c0516f75105";
            var doctor = A.Fake<Doctor>();
            A.CallTo(() => _doctorRepository.GetByIdAsync(id)).Returns(doctor);

            var result = _doctorController.Detail(id);

            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public void DoctorController_Search_ReturnsSuccess()
        {
            var doctors = A.Fake<IEnumerable<Doctor>>();
            A.CallTo(() => _doctorRepository.GetDoctorByNameOrCode("Ba")).Returns(doctors);

            var result = _doctorController.Search("Ba");

            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
