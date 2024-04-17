using Clinic.Data.Enum;
using Clinic.Models;
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

                if (!context.Doctors.Any())
                {
                    context.Doctors.AddRange(new List<Doctor>()
                    {
                        new Doctor()
                        {
                            Name = "Davide",
                            Surname = "Anderson",
                            Title = Title.Specialist,
                            Code = "DA74S"
                        },
                        new Doctor()
                        {
                            Name = "Ann",
                            Surname = "Brown",
                            Title = Title.Resident,
                            Code = "AB69R"
                        },
                        new Doctor()
                        {
                            Name = "Jully",
                            Surname = "Harris",
                            Title = Title.Nurse,
                            Code = "JH82N"
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.Receptions.Any())
                {
                    context.Receptions.AddRange(new List<Reception>() 
                    {
                        new Reception()
                        {
                            PatientId = 1, 
                            DoctorId = 1, 
                            DateTime = DateTime.Now.AddDays(15), 
                            Emergency = false 
                        },
                        new Reception()
                        {
                            PatientId = 2, 
                            DoctorId = 1, 
                            DateTime = DateTime.Now.AddDays(20),
                            Emergency = true 
                        },
                        new Reception()
                        {
                            PatientId = 3,
                            DoctorId = 1,
                            DateTime = DateTime.Now.AddDays(10),
                            Emergency = true
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
