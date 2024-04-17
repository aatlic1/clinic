using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Models
{
    public class Reception
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime DateTime { get; set; }
        public bool Emergency { get; set; }
    }
}
