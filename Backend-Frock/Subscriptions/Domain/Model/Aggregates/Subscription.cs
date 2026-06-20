using Backend_Frock.Subscriptions.Domain.Enums;
using Backend_Frock.Subscriptions.Domain.Model.Commands;

namespace Backend_Frock.Subscriptions.Domain.Model.Aggregates;

public class Subscription
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string PaypalSubscriptionId { get; private set; } = string.Empty;
    public string PaypalPlanId { get; private set; } = string.Empty;
    public SubscriptionStatus Status { get; private set; }
    public int MaxMembers { get; private set; }
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
        CompanyId = command.CompanyId;
        PaypalSubscriptionId = paypalSubscriptionId;
        PaypalPlanId = planId;
        Status = SubscriptionStatus.Pending;
        MaxMembers = 1;                                  // hasta activarse, tope free
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Activate()
    {
        Status = SubscriptionStatus.Active;
        MaxMembers = 10;                                 // plan pagado habilita 10
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow.AddMonths(1);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
        MaxMembers = 1;                                  // vuelve al tope free
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Renew()
    {
        Status = SubscriptionStatus.Active;
        MaxMembers = 10;
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow.AddMonths(1);
        UpdatedAt = DateTime.UtcNow;
    }
}