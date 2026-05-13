using Backend_Frock.Subscriptions.Domain.Enums;
using Backend_Frock.Subscriptions.Domain.Model.Commands;

namespace Backend_Frock.Subscriptions.Domain.Model.Aggregates;

public class Subscription
{
    public int Id { get; private set; }
    public int DriverId { get; private set; }
    public string PaypalSubscriptionId { get; private set; } = string.Empty;
    public string PaypalPlanId { get; private set; } = string.Empty;
    public SubscriptionStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
 
    public bool IsActive =>
        Status == SubscriptionStatus.Active &&
        EndDate > DateTime.UtcNow;
    
    protected Subscription() { }

    public Subscription(CreateSubscriptionCommand command, string paypalSubscriptionId, string planId)
    {
        DriverId = command.DriverId;
        PaypalSubscriptionId = paypalSubscriptionId;
        PaypalPlanId = planId;
        Status = SubscriptionStatus.Pending;
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Activate()
    {
        Status = SubscriptionStatus.Active;
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow.AddMonths(1);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Renew()
    {
        Status = SubscriptionStatus.Active;
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow.AddMonths(1);
        UpdatedAt = DateTime.UtcNow;
    }
}