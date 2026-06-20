using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Favorites.Domain.Repositories;

public interface IFavoriteRouteRepository : IBaseRepository<FavoriteRoute>
{
    Task<IEnumerable<FavoriteRoute>> FindByPassengerIdAsync(int passengerId);
    Task<FavoriteRoute?> FindByPassengerIdAndRouteIdAsync(int passengerId, int routeId);
}