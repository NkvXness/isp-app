namespace IspApp.Models
{
    // Оборудование, выданное абоненту (роутер или приставка)
    public class Equipment
    {
        public int Id { get; set; }

        public string Model { get; set; } = "";

        public string SerialNumber { get; set; } = "";

        public string EquipmentType { get; set; } = "";

        // статус: в аренде или продан
        public string Status { get; set; } = "";

        public int SubscriberId { get; set; }

        public Subscriber? Subscriber { get; set; }
    }
}