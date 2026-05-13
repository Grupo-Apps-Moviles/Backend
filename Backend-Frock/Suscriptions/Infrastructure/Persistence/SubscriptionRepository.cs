namespace Backend_Frock.Suscriptions.Infrastructure.Persistence;

using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Suscriptions.Domain.Model.Aggregates;
using Backend_Frock.Suscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context; // Tu DbContext de la aplicación

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task<Subscription?> GetByPaypalSubscriptionIdAsync(string paypalId)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.PaypalSubscriptionId == paypalId);
    }

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        return await _context.Subscriptions.FindAsync(id);
    }

    public void Update(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
        _context.SaveChanges();
    }
}