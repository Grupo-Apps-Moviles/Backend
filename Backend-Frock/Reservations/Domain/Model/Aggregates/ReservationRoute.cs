using Backend_Frock.Routes.Domain.Model.Aggregates;

namespace Backend_Frock.Reservations.Domain.Model.Aggregates;

public class ReservationRoute
{
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }

    public int RouteId { get; set; }
    public RouteAggregate Routes { get; set; }
}