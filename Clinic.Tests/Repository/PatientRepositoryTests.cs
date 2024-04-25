using Clinic.Data;
using Clinic.Data.Enum;
using Clinic.Models;
using Clinic.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Tests.Repository
{
    public class PatientRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Patients.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Patients.Add(
                        new Patient()
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
                            }
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void PatientRepository_Add_ReturnsBool()
        {
            var patient = new Patient()
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
                }
            };

            var dbContext = await GetDbContext();
            var patientRepository = new PatientRepository(dbContext);

            var result = patientRepository.Add(patient);

            result.Should().BeTrue();
        }

        [Fact]
        public async void PatientRepository_GetByIdAsync_ReturnsPatient()
        {
            var dbContext = await GetDbContext();
            var patientRepository = new PatientRepository(dbContext);

            var result = patientRepository.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Patient>>();
        }
        [Fact]  
        public async void PatientRepository_GetAll_ReturnsList()
        {
            var dbContext = await GetDbContext();
            var patientRepository = new PatientRepository(dbContext);

            var result = patientRepository.GetAll();

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Patient>>>();
        }
        [Fact]
        public async void PatientRepository_GetPatientByName_ReturnsPatients()
        {
            var dbContext = await GetDbContext();
            var patientRepository = new PatientRepository(dbContext);

            var result = patientRepository.GetPatientByName("Max");

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Patient>>>();
        }
        [Fact]
        public async void PatientRepository_SuccessfulDelete_ReturnsTrue()
        {
            var patient = new Patient()
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
                }
            };
            var dbContext = await GetDbContext();
            var patientRepository = new PatientRepository(dbContext);

            patientRepository.Add(patient);
            var result = patientRepository.Delete(patient);

            result.Should().BeTrue();
        }
        [Fact]
        public async void PatientRepository_SuccessfulUpdate_ReturnsTrue()
        {
            var patient = new Patient()
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
                }
            };
            var dbContext = await GetDbContext();
            var patientRepository = new PatientRepository(dbContext);

            patientRepository.Add(patient);
            patient.Name = "Nill";
            var result = patientRepository.Update(patient);

            result.Should().BeTrue();
        } 
    }
}
