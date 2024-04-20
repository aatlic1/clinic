using Clinic.Models;

namespace Clinic.ViewModels
{
    public class CreateReceptionViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public string DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime DateTime { get; set; }
        public bool Emergency { get; set; }
    }
}
