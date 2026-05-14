using Backend_Frock.Subscriptions.Domain.Model.Aggregates;
using Backend_Frock.Subscriptions.Domain.Model.Queries;

namespace Backend_Frock.Subscriptions.Domain.Service;

public interface ISubscriptionQueryService
{
    Task<Subscription?> Handle(GetSubscriptionByDriverIdQuery query);
    Task<Subscription?> Handle(GetSubscriptionByPaypalIdQuery query);
}