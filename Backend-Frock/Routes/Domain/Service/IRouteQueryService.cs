using Backend_Frock.Routes.Domain.Model.Queries;
using Backend_Frock.Routes.Domain.Model.Aggregates;
namespace Backend_Frock.Routes.Domain.Service
{
    public interface IRouteQueryService
    {
        Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesByFkCompanyIdQuery query);

        Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesQuery query);

        Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesByFkDistrictIdQuery query);

        Task<RouteAggregate?> Handle(GetRouteByIdQuery query);
    }
}
