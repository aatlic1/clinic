using Clinic.Data;
using Clinic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class ReceptionController : Controller
    {
        private readonly IReceptionRepository _receptionRepository;

        public ReceptionController(IReceptionRepository receptionRepository)
        {
            _receptionRepository = receptionRepository;
        }
        public async Task<IActionResult> Index()
        {
            var receptions = await _receptionRepository.GetAll();
            return View(receptions);
        }
    }
}
