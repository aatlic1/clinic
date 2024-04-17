using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
