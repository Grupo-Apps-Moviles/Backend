using Backend_Frock.Subscriptions.Domain.Model.Commands;

namespace Backend_Frock.Subscriptions.Domain.Service;

public interface ISubscriptionCommandService
{
    Task<string> Handle(CreateSubscriptionCommand command);
    
    Task HandleActivation(string paypalSubscriptionId);
    
    Task HandleCancellation(string paypalSubscriptionId);
    
    Task HandleRenewal(string paypalSubscriptionId);
}