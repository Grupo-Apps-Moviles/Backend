using Backend_Frock.Reservations.Domain.Model.Commands;
using Backend_Frock.Reservations.Interfaces.REST.Resources;

namespace Backend_Frock.Reservations.Interfaces.REST.Transform;

public static class CreateReservationCommandFromResourceAssembler
{
    public static CreateReservationCommand ToCommandFromResource(CreateReservationResource resource)
    {
        return new CreateReservationCommand(resource.UserId, resource.RouteIds);
    }
}