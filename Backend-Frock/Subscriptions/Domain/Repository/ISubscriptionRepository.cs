using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Subscriptions.Domain.Model.Aggregates;
using Backend_Frock.Subscriptions.Domain.Model.Queries;

namespace Backend_Frock.Subscriptions.Domain.Repository;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindByDriverIdAsync(GetSubscriptionByDriverIdQuery query);
    Task<Subscription?> FindByPaypalIdAsync(GetSubscriptionByPaypalIdQuery query);
}