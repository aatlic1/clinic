using Clinic.Data;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }
        public IActionResult Detail(int id)
        {
            Patient patient = _context.Patients.Include(a => a.Address).FirstOrDefault(p => p.Id == id); 
            return View(patient);
        }
    }
}
