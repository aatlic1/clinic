using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
