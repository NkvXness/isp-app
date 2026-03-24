using Microsoft.EntityFrameworkCore;

namespace IspApp.Models
{
    public class IspContext : DbContext
    {
        public IspContext(DbContextOptions<IspContext> options) : base(options)
        {
        }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
    }
}