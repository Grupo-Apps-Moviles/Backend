using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Model.Commands;

namespace Backend_Frock.Reservations.Domain.Services;

public interface IReservationCommandService
{
    Task<Reservation?> Handle(CreateReservationCommand command);
}