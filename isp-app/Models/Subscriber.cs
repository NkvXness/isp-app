namespace IspApp.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Address { get; set; } = "";
        public string Passport { get; set; } = "";
        // лицевой счёт абонента
        public string AccountNumber { get; set; } = "";
        public decimal Balance { get; set; }
        public int TariffId { get; set; }
        public Tariff? Tariff { get; set; }
        public List<Payment> Payments { get; set; } = new();
        public List<Equipment> Equipments { get; set; } = new();
        public List<SupportRequest> SupportRequests { get; set; } = new();
    }
}