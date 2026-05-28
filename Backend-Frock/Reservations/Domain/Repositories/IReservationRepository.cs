using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Reservations.Domain.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<IEnumerable<Reservation>> FindByUserIdAsync(int userId);
}