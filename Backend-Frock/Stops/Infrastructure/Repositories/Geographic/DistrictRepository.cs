using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;

using Microsoft.EntityFrameworkCore;


namespace Backend_Frock.Stops.Infrastructure.Repositories.Geographic
{
    public class DistrictRepository(AppDbContext context) : BaseStringRepository<District>(context), IDistrictRepository
    {
        public async Task<IEnumerable<District>> FindByFkIdProvinceAsync(int fkIdProvince)
        {
            return await Context.Set<District>()
                .Where(f => f.FkIdProvince == fkIdProvince)
                .ToListAsync();
        }
    }
}
