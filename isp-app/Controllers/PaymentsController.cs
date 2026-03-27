using IspApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IspApp.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IspContext _db;

        public PaymentsController(IspContext db)
        {
            _db = db;
        }

        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            var payments = _db.Payments
                .Include(p => p.Subscriber)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();

            return View(payments);
        }
    }
}