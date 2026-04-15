using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Model.Queries.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;
using Backend_Frock.Stops.Domain.Services.Geographic;

namespace Backend_Frock.Stops.Application.Internal.QueryServices.Geographic
{
    public class DistrictQueryService(IDistrictRepository districtRepository) : IDistrictQueryService
    {
        public async Task<IEnumerable<District>> Handle(GetAllDistrictsQuery query)
        {
            return await districtRepository.ListAsync();
        }
        public async Task<District?> Handle(GetDistrictByIdQuery query)
        {
            return await districtRepository.FindByIdIntAsync(query.Id);
        }        
        public async Task<IEnumerable<District>> Handle(GetDistrictsByFkIdProvinceQuery query)
        {
            return await districtRepository.FindByFkIdProvinceAsync(query.FkIdProvince);
        }
    }
}
