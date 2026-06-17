using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Model.Queries;

namespace Backend_Frock.Reservations.Domain.Services;

public interface IReservationQueryService
{
    Task<Reservation?> Handle(GetReservationByIdQuery query);
    Task<IEnumerable<Reservation>> Handle(GetAllReservationsByUserIdQuery query);
    Task<IEnumerable<Reservation>> Handle(GetReservationsByDriverIdQuery query);
}