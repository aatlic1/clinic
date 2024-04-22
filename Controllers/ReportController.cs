using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
