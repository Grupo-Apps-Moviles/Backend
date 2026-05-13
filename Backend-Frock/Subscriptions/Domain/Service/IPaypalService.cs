namespace Backend_Frock.Subscriptions.Domain.Service;

public interface IPaypalService
{
    Task<(string approvalUrl, string subscriptionId, string planId)> CreateSubscriptionAsync(int driverId);
 
    Task<bool> ValidateSubscriptionAsync(string paypalSubscriptionId);
}