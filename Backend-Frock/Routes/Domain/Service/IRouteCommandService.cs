using Backend_Frock.Routes.Domain.Model.Aggregates;
using Backend_Frock.Routes.Domain.Model.Commands;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Routes.Domain.Service
{
    public interface IRouteCommandService
    {
        Task<RouteAggregate?> Handle(CreateFullRouteCommand command);
        Task<RouteAggregate?> Handle(int IdRoute, UpdateRouteCommand command);
        Task Handle(DeleteRouteCommand command);
    }
}
