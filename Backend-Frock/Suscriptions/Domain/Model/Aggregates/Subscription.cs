namespace Backend_Frock.Suscriptions.Domain.Model.Aggregates
{
    public class Subscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PaypalSubscriptionId { get; set; } // ID retornado por PayPal
        public string Status { get; set; } // PENDING, ACTIVE, CANCELED
        public DateTime CreatedAt { get; set; }

        public Subscription(string userId, string paypalSubscriptionId)
        {
            UserId = userId;
            PaypalSubscriptionId = paypalSubscriptionId;
            Status = "PENDING";
            CreatedAt = DateTime.UtcNow;
        }
    }
}
