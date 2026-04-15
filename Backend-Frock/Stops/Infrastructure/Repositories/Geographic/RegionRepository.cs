using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;

namespace Backend_Frock.Stops.Infrastructure.Repositories.Geographic
{
    public class RegionRepository(AppDbContext context) : BaseStringRepository<Region>(context), IRegionRepository
    {
    }
}
