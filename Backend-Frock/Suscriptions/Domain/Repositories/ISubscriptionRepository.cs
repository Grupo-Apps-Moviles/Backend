using Backend_Frock.Suscriptions.Domain.Model.Aggregates;

namespace Backend_Frock.Suscriptions.Domain.Repositories
{
    public interface ISubscriptionRepository
    {
        Task AddAsync(Subscription subscription);
        Task<Subscription> GetByIdAsync(int id);
        Task<Subscription> GetByPaypalSubscriptionIdAsync(string paypalId);
        void Update(Subscription subscription);
    }
}
