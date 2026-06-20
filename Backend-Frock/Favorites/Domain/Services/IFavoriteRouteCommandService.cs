using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.Favorites.Domain.Model.Commands;

namespace Backend_Frock.Favorites.Domain.Services;

public interface IFavoriteRouteCommandService
{
    Task<FavoriteRoute?> Handle(CreateFavoriteRouteCommand command);
    Task<bool> Handle(DeleteFavoriteRouteCommand command);
}