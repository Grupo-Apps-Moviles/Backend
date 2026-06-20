using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.Favorites.Interfaces.REST.Resources;

namespace Backend_Frock.Favorites.Interfaces.REST.Transform;

public static class FavoriteRouteResourceFromEntityAssembler
{
    public static FavoriteRouteResource ToResourceFromEntity(FavoriteRoute entity)
    {
        return new FavoriteRouteResource(
            entity.Id,
            entity.PassengerId,
            entity.RouteId,
            entity.CreatedAt
        );
    }
}