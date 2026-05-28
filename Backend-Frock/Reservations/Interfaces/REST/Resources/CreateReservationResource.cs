namespace Backend_Frock.Reservations.Interfaces.REST.Resources;

public record CreateReservationResource(int UserId, List<int> RouteIds);