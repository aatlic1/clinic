using Clinic.Data;
using Clinic.Data.Enum;
using Clinic.Models;
using Clinic.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Tests.Repository
{
    public class ReportRepositoryTests
    {
        private readonly UserManager<Doctor> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ReportRepositoryTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            serviceCollection.AddLogging();
            serviceCollection.AddIdentity<Doctor, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _userManager = serviceProvider.GetRequiredService<UserManager<Doctor>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedRoles().Wait();
        }

        private async Task SeedRoles()
        {
            if (!await _roleManager.RoleExistsAsync(UserRole.Doctor))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Doctor));
        }
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Doctors.CountAsync() <= 0)
            {
                string adminUserEmail = "bathglares@gmail.com";
                var adminUser = await _userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new Doctor()
                    {
                        UserName = "bathglares",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Name = "Bath",
                        Surname = "Glares",
                        Code = "KOL89",
                        Title = Title.Resident
                    };
                    await _userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await _userManager.AddToRoleAsync(newAdminUser, UserRole.Doctor);
                }

                await databaseContext.SaveChangesAsync();
            }
            if (await databaseContext.Patients.CountAsync() <= 0)
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
            if (await databaseContext.Reports.CountAsync() <= 0)
            {
                databaseContext.Reports.Add(
                    new Report()
                    {
                        PatientId = 1,
                        DoctorId = "177895e5-61ae-45c0-a4d3-6440ce1e7dfa",
                        DateTime = new DateTime(new DateOnly(2024, 5, 5), new TimeOnly(12, 10)),
                        Description = "Something",
                        Caption = "Report",
                    }
                    );
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }
        [Fact]
        public async Task ReportRepository_Add_ReturnsBool()
        {
            var report = new Report()
            {
                PatientId = 1,
                DoctorId = "177895e5-61ae-45c0-a4d3-6440ce1e7dfa",
                DateTime = new DateTime(new DateOnly(2024, 5, 5), new TimeOnly(12, 10)),
                Description = "Something",
                Caption = "Report",
            };

            var dbContext = await GetDbContext();
            var reportRepository = new ReportRepository(dbContext);

            var result = reportRepository.Add(report);

            result.Should().BeTrue();
        }
        [Fact]
        public async Task ReportRepository_GetReportById_ReturnsReport()
        {
            var dbContext = await GetDbContext();
            var reportRepository = new ReportRepository(dbContext);

            var result = reportRepository.GetReportById(1);

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Report>>();
        }
        [Fact]
        public async Task ReportRepository_GetReportsByPatientId_ReturnsList()
        {
            var dbContext = await GetDbContext();
            var reportRepository = new ReportRepository(dbContext);

            var result = reportRepository.GetReportsByPatientId(1);

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Report>>>();
        }
        [Fact]
        public async Task ReportRepository_GetReportByReception_ReturnsReport()
        {
            var reception = new Reception()
            {
                PatientId = 1,
                DoctorId = "177895e5-61ae-45c0-a4d3-6440ce1e7dfa",
                DateTime = new DateTime(new DateOnly(2024, 6, 5), new TimeOnly(12, 10)),
                Emergency = false
            };

            var dbContext = await GetDbContext();
            var reportRepository = new ReportRepository(dbContext);

            var result = reportRepository.GetReportByReception(reception);

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Report>>();
        }
    }
}
