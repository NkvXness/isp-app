using IspApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IspApp.Controllers
{
    public class TariffsController : Controller
    {
        private readonly IspContext _db;

        public TariffsController(IspContext db)
        {
            _db = db;
        }

        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            var tariffs = _db.Tariffs
                .OrderBy(t => t.TariffType)
                .ThenBy(t => t.MonthlyPrice)
                .ToList();

            return View(tariffs);
        }
    }
}