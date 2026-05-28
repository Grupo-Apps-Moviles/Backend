namespace Backend_Frock.Reservations.Interfaces.REST.Resources;

public record ReservationResource(int Id, int UserId, string Status, List<int> RouteIds);