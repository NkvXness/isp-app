using IspApp.Models;
using IspApp.Models.ViewModels;
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

        // сколько абонентов находится на каждом тарифе
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Statistics()
        {
            var stats = _db.Tariffs
                .Select(t => new TariffStatsViewModel
                {
                    TariffName = t.Name,
                    TariffType = t.TariffType,
                    Speed = t.Speed,
                    MonthlyPrice = t.MonthlyPrice,
                    SubscriberCount = t.Subscribers.Count
                })
                .OrderByDescending(t => t.SubscriberCount)
                .ToList();

            return View(stats);
        }
    }
}