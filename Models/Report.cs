using System.ComponentModel.DataAnnotations;

namespace Clinic.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
