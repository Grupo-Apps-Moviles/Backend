using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;


using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;

using Microsoft.EntityFrameworkCore;


namespace Backend_Frock.Stops.Infrastructure.Repositories.Geographic
{
    public class ProvinceRepository(AppDbContext context) : BaseStringRepository<Province>(context), IProvinceRepository
    {
        public async Task<IEnumerable<Province>> FindByFkIdRegionAsync(int fkIdRegion)
        {
            return await Context.Set<Province>()
                .Where(f => f.FkIdRegion == fkIdRegion)
                .ToListAsync();
        }
    }
}
