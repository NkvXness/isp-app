using IspApp.Data;
using IspApp.Models;

namespace IspApp.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IspContext db)
        {
            DbInitializer.Initialize(db);
            await _next.Invoke(context);
        }
    }
}