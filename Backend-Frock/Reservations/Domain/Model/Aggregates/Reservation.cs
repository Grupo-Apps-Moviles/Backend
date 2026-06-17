using Backend_Frock.IAM.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Model.ValueObjects;

namespace Backend_Frock.Reservations.Domain.Model.Aggregates;

public partial class Reservation
{
    public int Id { get; }

    public int UserId { get; private set; }

    public User User { get; private set; }

    public ReservationStatus Status { get; private set; }

    public ICollection<ReservationRoute>
        ReservationRoutes { get; private set; }

    // PAYPAL
    public decimal Amount { get; private set; }

    public string PaypalTransactionId
    { get; private set; } = "";

    // GANANCIAS
    public int DriverId { get; private set; }

    public decimal DriverEarnings
    { get; private set; }

    public decimal PlatformFee
    { get; private set; }

    public DateTime CreatedAt
    { get; private set; }

    public Reservation()
    {
        ReservationRoutes =
            new List<ReservationRoute>();

        CreatedAt = DateTime.UtcNow;

        Status =
            ReservationStatus.PendingPayment;
    }

    public Reservation(
        int userId,
        List<int> routeIds,
        decimal amount,
        string paypalTransactionId,
        int driverId
    ) : this()
    {
        UserId = userId;

        Amount = amount;

        PaypalTransactionId =
            paypalTransactionId;

        DriverId = driverId;

        DriverEarnings = amount * 1;

        PlatformFee = amount * 0;

        Status = ReservationStatus.Paid;

        foreach (var routeId in routeIds)
        {
            ReservationRoutes.Add(
                new ReservationRoute
                {
                    RouteId = routeId
                });
        }
    }

    public void CancelReservation()
    {
        Status = ReservationStatus.Canceled;
    }
}