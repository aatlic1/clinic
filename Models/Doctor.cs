using Clinic.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Models
{
    public class Doctor : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Title? Title { get; set; }
        public string? Code { get; set; }
    }
}
