using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Model.Queries.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;
using Backend_Frock.Stops.Domain.Services.Geographic;

namespace Backend_Frock.Stops.Application.Internal.QueryServices.Geographic
{
    public class ProvinceQueryService(IProvinceRepository provinceRepository) : IProvinceQueryService
    {
        public async Task<IEnumerable<Province>> Handle(GetAllProvincesQuery query)
        {
            return await provinceRepository.ListAsync();
        }
        public async Task<Province?> Handle(GetProvinceByIdQuery query)
        {
            return await provinceRepository.FindByIdIntAsync(query.Id);
        }

        public async Task<IEnumerable<Province>> Handle(GetProvincesByFkIdRegionQuery query)
        {
            return await provinceRepository.FindByFkIdRegionAsync(query.FkIdRegion);
        }
    }
}
