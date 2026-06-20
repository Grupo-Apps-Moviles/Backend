using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.Favorites.Domain.Model.Queries;
using Backend_Frock.Favorites.Domain.Repositories;
using Backend_Frock.Favorites.Domain.Services;

namespace Backend_Frock.Favorites.Application.Internal.QueryServices;

public class FavoriteRouteQueryService : IFavoriteRouteQueryService
{
    private readonly IFavoriteRouteRepository _favoriteRouteRepository;

    public FavoriteRouteQueryService(IFavoriteRouteRepository favoriteRouteRepository)
    {
        _favoriteRouteRepository = favoriteRouteRepository;
    }

    public async Task<FavoriteRoute?> Handle(GetFavoriteRouteByIdQuery query)
    {
        return await _favoriteRouteRepository.FindByIdAsync(query.FavoriteRouteId);
    }

    public async Task<IEnumerable<FavoriteRoute>> Handle(GetAllFavoriteRoutesByPassengerIdQuery query)
    {
        return await _favoriteRouteRepository.FindByPassengerIdAsync(query.PassengerId);
    }
}