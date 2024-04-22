using Clinic.Models;

namespace Clinic.ViewModels
{
    public class CreateReportViewModel
    {
        public int? Id { get; set; }
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }
        public string? DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string? Description { get; set; }
        public string? Caption { get; set; }
        public DateTime DateTime { get; set; }
    }
}
