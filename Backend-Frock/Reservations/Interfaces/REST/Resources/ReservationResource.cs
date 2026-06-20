namespace Backend_Frock.Reservations.Interfaces.REST.Resources;

public record ReservationResource(
    int Id,
    int UserId,
    int DriverId,
    string Status,
    decimal Amount,
    decimal DriverEarnings,
    decimal PlatformFee,
    string PaypalTransactionId,
    DateTime CreatedAt
);