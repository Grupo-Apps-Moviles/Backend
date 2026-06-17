namespace Backend_Frock.Reservations.Interfaces.REST.Resources;

public record CreateReservationResource(
    int UserId,
    List<int> RouteIds,
    decimal Amount,
    string PaypalTransactionId,
    int DriverId
);