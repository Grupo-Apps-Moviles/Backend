using Backend_Frock.Favorites.Domain.Model.Commands;
using Backend_Frock.Favorites.Interfaces.REST.Resources;

namespace Backend_Frock.Favorites.Interfaces.REST.Transform;

public static class CreateFavoriteRouteCommandFromResourceAssembler
{
    public static CreateFavoriteRouteCommand ToCommandFromResource(CreateFavoriteRouteResource resource)
    {
        return new CreateFavoriteRouteCommand(resource.PassengerId, resource.RouteId);
    }
}
