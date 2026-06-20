namespace Backend_Frock.Reservations.Domain.Model.Commands;

public record CreateReservationCommand(
    int UserId,
    List<int> RouteIds,
    decimal Amount,
    string PaypalTransactionId,
    int DriverId
);