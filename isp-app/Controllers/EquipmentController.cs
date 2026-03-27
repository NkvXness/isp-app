using IspApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IspApp.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IspContext _db;

        public EquipmentController(IspContext db)
        {
            _db = db;
        }

        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            var equipment = _db.Equipment
                .Include(e => e.Subscriber)
                .OrderBy(e => e.EquipmentType)
                .ThenBy(e => e.Model)
                .ToList();

            return View(equipment);
        }
    }
}