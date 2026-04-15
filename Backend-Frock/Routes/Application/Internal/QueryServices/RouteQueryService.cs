using Backend_Frock.Routes.Domain.Model.Aggregates;
using Backend_Frock.Routes.Domain.Model.Queries;
using Backend_Frock.Routes.Domain.Repository;
using Backend_Frock.Routes.Domain.Service;

namespace Backend_Frock.Routes.Application.Internal.QueryServices
{
    public class RouteQueryService(IRouteRepository routeRepository) : IRouteQueryService
    {
        public async Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesByFkCompanyIdQuery query)
        {
            try
            {
                return await routeRepository.FindByCompanyId(query.FkCompanyId);
            }
            catch (Exception e)
            {

                throw new Exception($"Error retrieving routes for company: {e.Message}", e);
            }
        }

        public async Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesQuery query)
        {
            try
            {
                return await routeRepository.ListRoutes();
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving all routes: {e.Message}", e);
            }
        }

        public async Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesByFkDistrictIdQuery query)
        {
            try
            {
                return await routeRepository.FindByDistrictId(query.FkDistrictId);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving routes for district: {e.Message}", e);
            }
        }

        public async Task<RouteAggregate?> Handle(GetRouteByIdQuery query)
        {
            try
            {
                return await routeRepository.FindByRouteId(query.Id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving route by ID: {e.Message}", e);
            }

        }
    }
}
