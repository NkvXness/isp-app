using IspApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IspApp.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly IspContext _db;

        public SubscribersController(IspContext db)
        {
            _db = db;
        }

        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            // подгрузка тарифа сразу, чтобы не делать отдельные запросы для каждой строки
            var subscribers = _db.Subscribers
                .Include(s => s.Tariff)
                .OrderBy(s => s.FullName)
                .ToList();

            return View(subscribers);
        }

        // список абонентов с отрицательным балансом
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Debtors()
        {
            var debtors = _db.Subscribers
                .Include(s => s.Tariff)
                .Where(s => s.Balance < 0)
                .OrderBy(s => s.Balance)
                .ToList();

            return View(debtors);
        }
    }
}