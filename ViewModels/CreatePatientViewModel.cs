using Clinic.Data.Enum;
using Clinic.Models;

namespace Clinic.ViewModels
{
    public class CreatePatientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
