using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Repositories;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend_Frock.Reservations.Infrastructure.Repositories;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Reservation>> FindByUserIdAsync(int userId)
    {
        return await Context.Set<Reservation>()
            .Include(r => r.ReservationRoutes)
            .ThenInclude(rr => rr.Routes)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }
    
    // Sobrescribimos FindByIdAsync para incluir las rutas relacionadas
    public new async Task<Reservation?> FindByIdAsync(int id)
    {
        return await Context.Set<Reservation>()
            .Include(r => r.ReservationRoutes)
            .ThenInclude(rr => rr.Routes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}