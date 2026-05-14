using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Subscriptions.Domain.Model.Aggregates;
using Backend_Frock.Subscriptions.Domain.Model.Commands;
using Backend_Frock.Subscriptions.Domain.Model.Queries;
using Backend_Frock.Subscriptions.Domain.Model.Repository;
using Backend_Frock.Subscriptions.Domain.Service;

namespace Backend_Frock.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPaypalService paypalService,
    IUnitOfWork unitOfWork) : ISubscriptionCommandService
{
    public async Task<string?> Handle(CreateSubscriptionCommand command)
    {
        var existing = await subscriptionRepository
            .FindByDriverIdAsync(new GetSubscriptionByDriverIdQuery(command.DriverId));
 
        if (existing is not null && existing.IsActive)
            return null;
 
        try
        {
            // La tupla ahora coincide exactamente con IPaypalService
            var (approvalUrl, subscriptionId, planId) =
                await paypalService.CreateSubscriptionAsync(command.DriverId);
 
            var subscription = new Subscription(command, subscriptionId, planId);
            await subscriptionRepository.AddAsync(subscription);
            await unitOfWork.CompleteAsync();
 
            return approvalUrl;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error creating PayPal subscription: {e.Message}");
            return null;
        }
    }
 
    public async Task HandleActivation(string paypalSubscriptionId)
    {
        var subscription = await subscriptionRepository
            .FindByPaypalIdAsync(new GetSubscriptionByPaypalIdQuery(paypalSubscriptionId));
 
        if (subscription is null) return;
 
        subscription.Activate();
        subscriptionRepository.Update(subscription); // Update viene de IBaseRepository ✅
        await unitOfWork.CompleteAsync();
    }
 
    public async Task HandleCancellation(string paypalSubscriptionId)
    {
        var subscription = await subscriptionRepository
            .FindByPaypalIdAsync(new GetSubscriptionByPaypalIdQuery(paypalSubscriptionId));
 
        if (subscription is null) return;
 
        subscription.Cancel();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
    }
 
    public async Task HandleRenewal(string paypalSubscriptionId)
    {
        var subscription = await subscriptionRepository
            .FindByPaypalIdAsync(new GetSubscriptionByPaypalIdQuery(paypalSubscriptionId));
 
        if (subscription is null) return;
 
        subscription.Renew();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
    }
}