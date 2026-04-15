using Backend_Frock.Routes.Domain.Model.Aggregates;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Routes.Domain.Repository
{
    public interface IRouteRepository:IBaseRepository<RouteAggregate>
    {
        Task<List<RouteAggregate>> FindByCompanyId(int companyId);
        Task<List<RouteAggregate>> FindByDistrictId(int districtId);

        Task<List<RouteAggregate>> ListRoutes();

        Task<RouteAggregate?> FindByRouteId(int id);
    }
}
