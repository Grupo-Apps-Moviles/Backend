using Backend_Frock.Routes.Domain.Model.Aggregates;
using Backend_Frock.Routes.Domain.Repository;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend_Frock.Routes.Infrastructure.Repositories
{
    public class RouteRepository(AppDbContext context) : BaseRepository<RouteAggregate>(context), IRouteRepository
    {
        public Task<List<RouteAggregate>> FindByCompanyId(int companyId)
        {
            return Context.Set<RouteAggregate>()
            // Incluimos Stops → Stop para poder filtrar por FkIdCompany
            .Include(r => r.Stops)
                .ThenInclude(rs => rs.Stop)
            .Include(r => r.Schedules)
            .Where(r => r.Stops.Any(rs => rs.Stop.FkIdCompany == companyId))
            .ToListAsync();
        }

        public Task<List<RouteAggregate>> FindByDistrictId(int districtId)
        {
            return Context.Set<RouteAggregate>()
            // Incluimos Stops → Stop para poder filtrar por FkIdDistrict
            .Include(r => r.Stops)
                .ThenInclude(rs => rs.Stop)
            .Include(r => r.Schedules)
            .Where(r => r.Stops.Any(rs => rs.Stop.FkIdDistrict == districtId))
            .ToListAsync();
        }

        public Task<List<RouteAggregate>> ListRoutes()
        {
            return Context.Set<RouteAggregate>()
            .Include(r => r.Stops)
                .ThenInclude(rs => rs.Stop)
            .Include(r => r.Schedules)
            .ToListAsync();
        }

        public Task<RouteAggregate?> FindByRouteId(int id)
        {
            return Context.Set<RouteAggregate>()
            .Include(r => r.Stops)
                .ThenInclude(rs => rs.Stop)
            .Include(r => r.Schedules)
            .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
