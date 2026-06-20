using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.Favorites.Domain.Repositories;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend_Frock.Favorites.Infrastructure.Repositories;

public class FavoriteRouteRepository : BaseRepository<FavoriteRoute>, IFavoriteRouteRepository
{
    public FavoriteRouteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<FavoriteRoute>> FindByPassengerIdAsync(int passengerId)
    {
        return await Context.Set<FavoriteRoute>()
            .Where(f => f.PassengerId == passengerId)
            .ToListAsync();
    }

    public async Task<FavoriteRoute?> FindByPassengerIdAndRouteIdAsync(int passengerId, int routeId)
    {
        return await Context.Set<FavoriteRoute>()
            .FirstOrDefaultAsync(f => f.PassengerId == passengerId && f.RouteId == routeId);
    }
}
