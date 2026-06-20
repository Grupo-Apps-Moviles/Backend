using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.Favorites.Domain.Model.Queries;

namespace Backend_Frock.Favorites.Domain.Services;

public interface IFavoriteRouteQueryService
{
    Task<FavoriteRoute?> Handle(GetFavoriteRouteByIdQuery query);
    Task<IEnumerable<FavoriteRoute>> Handle(GetAllFavoriteRoutesByPassengerIdQuery query);
}