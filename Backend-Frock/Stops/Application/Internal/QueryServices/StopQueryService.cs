using Backend_Frock.Stops.Domain.Model.Aggregates;
using Backend_Frock.Stops.Domain.Model.Queries;
using Backend_Frock.Stops.Domain.Repositories;
using Backend_Frock.Stops.Domain.Services;

namespace Backend_Frock.Stops.Application.Internal.QueryServices
{
    public class StopQueryService(IStopRepository stopRepository) : IStopQueryService
    {

        public async Task<IEnumerable<Stop>> Handle(GetAllStopsByFkIdCompanyQuery query)
        {
            return await stopRepository.FindByFkIdCompanyAsync(query.FkIdCompany);
        }
        public async Task<IEnumerable<Stop>> Handle(GetAllStopsByFkIdDistrictQuery query)
        {
            return await stopRepository.FindByFkIdDistrictAsync(query.FkIdDistrict);
        }
        public async Task<Stop?> Handle(GetStopByIdQuery query)
        {
            return await stopRepository.FindByIdAsync(query.Id);
        }
        public async Task<Stop?> Handle(GetStopByNameAndFkIdDistrictQuery query)
        {
            return await stopRepository.FindByNameAndFkIdDistrictAsync(query.Name, query.FkIdDistrict);
        }

        public async Task<IEnumerable<Stop>> Handle(GetAllStopsQuery query)
        {
            return await stopRepository.ListAsync();
        }

        public async Task<Stop?> Handle(GetStopByNameAndFkIdCompanyQuery query)
        {
            return await stopRepository.FindByNameAndFkIdCompanyAsync(query.Name, query.FkIdCompany);

        }
    }
}
