using Clinic.Data;
using Microsoft.AspNetCore.Mvc;

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
    }
}
