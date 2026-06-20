namespace Backend_Frock.Favorites.Domain.Model.Aggregates;

public class FavoriteRoute
{
    public int Id { get; }
    public int PassengerId { get; private set; }
    public int RouteId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public FavoriteRoute()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public FavoriteRoute(int passengerId, int routeId) : this()
    {
        PassengerId = passengerId;
        RouteId = routeId;
    }
}
