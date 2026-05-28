using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Interfaces.REST.Resources;

namespace Backend_Frock.Reservations.Interfaces.REST.Transform;

public static class ReservationResourceFromEntityAssembler
{
    public static ReservationResource ToResourceFromEntity(Reservation entity)
    {
        var routeIds = entity.ReservationRoutes.Select(rr => rr.RouteId).ToList();
        return new ReservationResource(entity.Id, entity.UserId, entity.Status.ToString(), routeIds);
    }
}