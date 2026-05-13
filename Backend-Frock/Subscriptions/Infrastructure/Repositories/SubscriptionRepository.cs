using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Backend_Frock.Subscriptions.Domain.Model.Aggregates;
using Backend_Frock.Subscriptions.Domain.Model.Queries;
using Backend_Frock.Subscriptions.Domain.Model.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend_Frock.Subscriptions.Infrastructure.Repositories;
 
public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public Task<Subscription?> FindByDriverIdAsync(GetSubscriptionByDriverIdQuery query)
    {
        return Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.DriverId == query.DriverId);
    }
 
    public Task<Subscription?> FindByPaypalIdAsync(GetSubscriptionByPaypalIdQuery query)
    {
        return Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.PaypalSubscriptionId == query.PaypalSubscriptionId);
    }
}