namespace IspApp.Models
{
    // Заявка абонента в тех. поддержку
    public class SupportRequest
    {
        public int Id { get; set; }

        public DateTime RequestDate { get; set; }

        public string Problem { get; set; } = "";

        // заявка открыта или закрыта
        public string Status { get; set; } = "";

        // мастер может быть не назначен
        public string? Master { get; set; }

        public int SubscriberId { get; set; }

        public Subscriber? Subscriber { get; set; }
    }
}