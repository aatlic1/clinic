using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class ReceptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
