using Backend_Frock.IAM.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Model.ValueObjects;

namespace Backend_Frock.Reservations.Domain.Model.Aggregates;

public partial class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ReservationStatus Status { get; set; }
    public ICollection<ReservationRoute> ReservationRoutes { get; set; }

    public Reservation()
    {
        ReservationRoutes = new List<ReservationRoute>();
    }

    public Reservation(int userId, List<int> routeIds) : this()
    {
        UserId = userId;
        Status = ReservationStatus.Reserved;
        
        foreach (var routeId in routeIds)
        {
            ReservationRoutes.Add(new ReservationRoute { RouteId = routeId });
        }
    }

    public void CancelReservation()
    {
        Status = ReservationStatus.Canceled; 
    }
}