using Clinic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class ReceptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceptionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var receptions = _context.Receptions.Include(p => p.Patient).Include(d => d.Doctor).ToList();
            return View(receptions);
        }
    }
}
