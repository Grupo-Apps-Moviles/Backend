namespace Backend_Frock.Favorites.Domain.Model.Commands;

public record CreateFavoriteRouteCommand(int PassengerId, int RouteId);