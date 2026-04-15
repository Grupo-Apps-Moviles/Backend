using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Model.Queries.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;
using Backend_Frock.Stops.Domain.Services.Geographic;

namespace Backend_Frock.Stops.Application.Internal.QueryServices.Geographic
{
    public class RegionQueryService(IRegionRepository regionRepository) : IRegionQueryService
    {
        public async Task<IEnumerable<Region>> Handle(GetAllRegionsQuery query)
        {
            return await regionRepository.ListAsync();
        }
        public async Task<Region?> Handle(GetRegionByIdQuery query)
        {
            return await regionRepository.FindByIdIntAsync(query.Id);
        }
    }
}
