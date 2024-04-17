using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
