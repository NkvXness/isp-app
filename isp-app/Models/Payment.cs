namespace IspApp.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public int SubscriberId { get; set; }

        public Subscriber? Subscriber { get; set; }
    }
}