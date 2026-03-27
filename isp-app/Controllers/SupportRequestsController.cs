using IspApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IspApp.Controllers
{
    public class SupportRequestsController : Controller
    {
        private readonly IspContext _db;

        public SupportRequestsController(IspContext db)
        {
            _db = db;
        }

        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            var requests = _db.SupportRequests
                .Include(r => r.Subscriber)
                .OrderByDescending(r => r.RequestDate)
                .ToList();

            return View(requests);
        }
    }
}