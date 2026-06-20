using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Interfaces.REST.Resources;

namespace Backend_Frock.Reservations.Interfaces.REST.Transform;

public static class
    ReservationResourceFromEntityAssembler
{
    public static ReservationResource
        ToResourceFromEntity(
            Reservation entity)
    {
        return new ReservationResource(
            entity.Id,
            entity.UserId,
            entity.DriverId,
            entity.Status.ToString(),
            entity.Amount,
            entity.DriverEarnings,
            entity.PlatformFee,
            entity.PaypalTransactionId,
            entity.CreatedAt
        );
    }
}