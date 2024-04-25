using Clinic.Data.Enum;
using Clinic.Data;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Clinic.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using FakeItEasy;

namespace Clinic.Tests.Repository
{
    public class ReceptionRepositoryTests
    {
        private readonly UserManager<Doctor> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceptionRepositoryTests()
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

            _httpContextAccessor = A.Fake<HttpContextAccessor>();
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
            if (await databaseContext.Receptions.CountAsync() <= 0)
            {
                databaseContext.Receptions.Add(
                    new Reception()
                    {
                        PatientId = 1,
                        DoctorId = "177895e5-61ae-45c0-a4d3-6440ce1e7dfa",
                        DateTime = new DateTime(new DateOnly(2024, 5, 5), new TimeOnly(12, 10)),
                        Emergency = false
                    }
                    ) ;
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }
        [Fact]
        public async Task ReceptionRepository_Add_ReturnsBool()
        {
            var reception = new Reception()
            {
                PatientId = 1,
                DoctorId = "177895e5-61ae-45c0-a4d3-6440ce1e7dfa",
                DateTime = new DateTime(new DateOnly(2024, 6, 5), new TimeOnly(12, 10)),
                Emergency = false
            };

            var dbContext = await GetDbContext();
            var receptionRepository = new ReceptionRepository(dbContext, _httpContextAccessor);

            var result = receptionRepository.Add(reception);

            result.Should().BeTrue();
        }
        [Fact]
        public async Task ReceptionRepository_GetByIdAsync_ReturnsReception()
        {
            var dbContext = await GetDbContext();
            var receptionRepository = new ReceptionRepository(dbContext, _httpContextAccessor);

            var result = receptionRepository.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Reception>>();
        }
        [Fact]
        public async Task ReceptionRepository_GetAll_ReturnsList()
        {
            var dbContext = await GetDbContext();
            var receptionRepository = new ReceptionRepository(dbContext, _httpContextAccessor);

            var result = receptionRepository.GetAll();

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Reception>>>();
        }
        [Fact]
        public async Task ReceptionRepository_GetReceptionsByDates_ReturnsList()
        {
            var dbContext = await GetDbContext();
            var receptionRepository = new ReceptionRepository(dbContext, _httpContextAccessor);

            var result = receptionRepository.GetReceptionsByDates(new DateTime(2024,4,4), new DateTime(2024, 6, 3));

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Reception>>>();
        }
        [Fact]
        public async Task ReceptionRepository_SuccessfulDelete_ReturnsBool()
        {
            var reception = new Reception()
            {
                PatientId = 1,
                DoctorId = "177895e5-61ae-45c0-a4d3-6440ce1e7dfa",
                DateTime = new DateTime(new DateOnly(2024, 6, 4), new TimeOnly(12, 10)),
                Emergency = false
            };

            var dbContext = await GetDbContext();
            var receptionRepository = new ReceptionRepository(dbContext, _httpContextAccessor);

            receptionRepository.Add(reception);
            var result = receptionRepository.Delete(reception);

            result.Should().BeTrue();
        }
        [Fact]
        public async Task ReceptionRepository_SuccessfulUpdate_ReturnsBool()
        {
            var reception = new Reception()
            {
                PatientId = 1,
                DoctorId = "177895e5-61ae-45c0-a4d3-6440ce1e7dfa",
                DateTime = new DateTime(new DateOnly(2024, 6, 4), new TimeOnly(12, 10)),
                Emergency = false
            };

            var dbContext = await GetDbContext();
            var receptionRepository = new ReceptionRepository(dbContext, _httpContextAccessor);

            receptionRepository.Add(reception);
            reception.Emergency = true;
            var result = receptionRepository.Update(reception);

            result.Should().BeTrue();
        }
    }
}
