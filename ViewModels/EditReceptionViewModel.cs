using Clinic.Models;

namespace Clinic.ViewModels
{
    public class EditReceptionViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime DateTime { get; set; }
        public bool Emergency { get; set; }
    }
}
