using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Model.Queries;
using Backend_Frock.Reservations.Domain.Repositories;
using Backend_Frock.Reservations.Domain.Services;

namespace Backend_Frock.Reservations.Application.Internal.QueryServices;

public class ReservationQueryService : IReservationQueryService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationQueryService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<Reservation?> Handle(GetReservationByIdQuery query)
    {
        return await _reservationRepository.FindByIdAsync(query.ReservationId);
    }

    public async Task<IEnumerable<Reservation>> Handle(GetAllReservationsByUserIdQuery query)
    {
        return await _reservationRepository.FindByUserIdAsync(query.UserId);
    }
    
    public async Task<IEnumerable<Reservation>> Handle(GetReservationsByDriverIdQuery query)
    {
        return await _reservationRepository
            .FindByDriverIdAsync(
                query.DriverId);
    }
}