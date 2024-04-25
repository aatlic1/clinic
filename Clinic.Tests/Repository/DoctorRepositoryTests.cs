using Clinic.Data.Enum;
using Clinic.Data;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Clinic.Repository;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.Tests.Repository
{
    public class DoctorRepositoryTests
    {
        private readonly UserManager<Doctor> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DoctorRepositoryTests()
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
            await databaseContext.Database.EnsureCreatedAsync();

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
            return databaseContext;
        }

        [Fact]
        public async Task DoctorRepository_Add_ReturnsBool()
        {
            var doctor = new Doctor()
            {
                UserName = "bathglares",
                Email = "bathglares@gmail.com",
                EmailConfirmed = true,
                Name = "Bath",
                Surname = "Glares",
                Code = "KOL89",
                Title = Title.Resident
            };

            var dbContext = await GetDbContext();
            var doctorRepository = new DoctorRepository(dbContext);

            var result = doctorRepository.Add(doctor);

            result.Should().BeTrue();
        }
        [Fact]
        public async void DoctorRepository_GetAll_ReturnsList()
        {
            var dbContext = await GetDbContext();
            var doctorRepository = new DoctorRepository(dbContext);

            var result = doctorRepository.GetAll();

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Doctor>>>();
        }
        [Fact]
        public async void DoctorRepository_GetDoctorByNameOrCode_ReturnsDoctors()
        {
            var dbContext = await GetDbContext();
            var doctorRepository = new DoctorRepository(dbContext);

            var result = doctorRepository.GetDoctorByNameOrCode("Bath");

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Doctor>>>();
        }
        [Fact]
        public async void DoctorRepository_GetSpecialistAndResident_ReturnsList()
        {
            var dbContext = await GetDbContext();
            var doctorRepository = new DoctorRepository(dbContext);

            var result = doctorRepository.GetSpecialistAndResident();

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<IEnumerable<Doctor>>>();
        }
        [Fact]
        public async void DoctorRepository_SuccessfulDelete_ReturnsTrue()
        {
            var doctor = new Doctor()
            {
                UserName = "mathglares",
                Email = "mathglares@gmail.com",
                EmailConfirmed = true,
                Name = "Math",
                Surname = "Glares",
                Code = "KOL83",
                Title = Title.Resident
            };
            var dbContext = await GetDbContext();
            var doctorRepository = new DoctorRepository(dbContext);

            doctorRepository.Add(doctor);
            var result = doctorRepository.Delete(doctor);

            result.Should().BeTrue();
        }
        [Fact]
        public async void DoctorRepository_SuccessfulUpdate_ReturnsTrue()
        {
            var doctor = new Doctor()
            {
                UserName = "nataliglares",
                Email = "natalilares@gmail.com",
                EmailConfirmed = true,
                Name = "Natali",
                Surname = "Glares",
                Code = "KoL83",
                Title = Title.Resident
            };
            var dbContext = await GetDbContext();
            var doctorRepository = new DoctorRepository(dbContext);

            doctorRepository.Add(doctor);
            doctor.Code = "lock895J";
            var result = doctorRepository.Update(doctor);

            result.Should().BeTrue();
        }
    }
}
