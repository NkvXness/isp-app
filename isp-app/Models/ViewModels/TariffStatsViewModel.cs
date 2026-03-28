namespace IspApp.Models.ViewModels
{
    public class TariffStatsViewModel
    {
        public string TariffName { get; set; } = "";
        public string TariffType { get; set; } = "";
        public int Speed { get; set; }
        public decimal MonthlyPrice { get; set; }
        public int SubscriberCount { get; set; }
    }
}