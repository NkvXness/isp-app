namespace IspApp.Models
{
    // Тариф интернет-провайдера
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Speed { get; set; }
        public decimal MonthlyPrice { get; set; }
        public string TariffType { get; set; } = "";
        public List<Subscriber> Subscribers { get; set; } = new();
    }
}