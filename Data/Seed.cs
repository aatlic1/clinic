using Clinic.Data.Enum;
using Clinic.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Clinic.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Patients.Any())
                {
                    context.Patients.AddRange(new List<Patient>()
                    {
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
                         },
                        new Patient()
                        {
                            Name = "Jane",
                            Surname = "Brown",
                            BirthDate = new DateOnly(1995, 5, 10),
                            Gender = Gender.Female,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            },
                            PhoneNumber = "000/000-001"
                         },
                        new Patient()
                        {
                            Name = "Marry",
                            Surname = "Martinez",
                            BirthDate = new DateOnly(1963, 11, 13),
                            Gender = Gender.Unknown,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                         },
                        new Patient()
                        {
                            Name = "John",
                            Surname = "White",
                            BirthDate = new DateOnly(1959, 12, 31),
                            Gender = Gender.Male,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                         },
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRole.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                if (!await roleManager.RoleExistsAsync(UserRole.Doctor))
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Doctor));
                if (!await roleManager.RoleExistsAsync(UserRole.Nurse))
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Nurse));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Doctor>>();
                string adminUserEmail = "maxsmith@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new Doctor()
                    {
                        UserName = "maxsmith",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Name = "Max",
                        Surname = "Smith"
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRole.Admin);
                }

                string appDoctorEmail = "annbrown@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appDoctorEmail);
                if (appUser == null)
                {
                    var newAppUser = new Doctor()
                    {
                        UserName = "annbrown",
                        Email = appDoctorEmail,
                        EmailConfirmed = true,
                        Name = "Ann",
                        Surname = "Brown",
                        Title = Title.Specialist,
                        Code = "DA74S"
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRole.Doctor);
                }

                string appNurseEmail = "jullyharris@gmail.com";

                var appUserNurse = await userManager.FindByEmailAsync(appNurseEmail);
                if (appUserNurse == null)
                {
                    var newAppUser = new Doctor()
                    {
                        UserName = "jullyharris",
                        Email = appNurseEmail,
                        EmailConfirmed = true,
                        Name = "Jully",
                        Surname = "Harris",
                        Title = Title.Nurse,
                        Code = "kol885M"
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRole.Nurse);
                }

            }
        }
    }
}
