using Clinic.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Title Title { get; set; }
        public string Code { get; set; }
    }
}
