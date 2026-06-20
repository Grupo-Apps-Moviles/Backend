using Backend_Frock.Subscriptions.Domain.Model.Aggregates;
using Backend_Frock.Subscriptions.Domain.Model.Queries;
using Backend_Frock.Subscriptions.Domain.Model.Repository;
using Backend_Frock.Subscriptions.Domain.Service;

namespace Backend_Frock.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository)
    : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(GetSubscriptionByCompanyIdQuery query)
    {
        try
        {
            return await subscriptionRepository.FindByCompanyIdAsync(query);
        }
        catch (Exception e)
        {
            throw new Exception($"Error retrieving subscription for company: {e.Message}", e);
        }
    }
 
    public async Task<Subscription?> Handle(GetSubscriptionByPaypalIdQuery query)
    {
        try
        {
            return await subscriptionRepository.FindByPaypalIdAsync(query);
        }
        catch (Exception e)
        {
            throw new Exception($"Error retrieving subscription by PayPal ID: {e.Message}", e);
        }
    }
}