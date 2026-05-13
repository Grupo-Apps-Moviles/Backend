namespace Backend_Frock.Suscriptions.Application.Internal.QueryServices;

using Backend_Frock.Suscriptions.Domain.Model.Aggregates;
using Backend_Frock.Suscriptions.Domain.Repositories;

public class SubscriptionQueryService
{
    private readonly ISubscriptionRepository _repository;

    public SubscriptionQueryService(ISubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Subscription?> GetSubscriptionById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Subscription?> GetByPaypalId(string paypalId)
    {
        return await _repository.GetByPaypalSubscriptionIdAsync(paypalId);
    }
}