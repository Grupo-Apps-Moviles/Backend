using Backend_Frock.Routes.Domain.Model.Aggregates;
using Backend_Frock.Routes.Domain.Model.Commands;
using Backend_Frock.Routes.Domain.Repository;
using Backend_Frock.Routes.Domain.Service;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Routes.Application.Internal.CommandServices
{
    public class RouteCommandService(IRouteRepository routeRepository, IUnitOfWork unitOfWork):IRouteCommandService
    {
        public async Task<RouteAggregate?> Handle(CreateFullRouteCommand command)
        {

            var newRoute = new RouteAggregate(command);
            try
            {
                await routeRepository.AddAsync(newRoute);
                await unitOfWork.CompleteAsync();
                return newRoute;
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error creating stop with name {StopName} for locality {LocalityId}.", command.Name, command.FkIdLocality);
                return null; // Signal failure to the controller
            }
        }
        public async Task<RouteAggregate?> Handle(int idRoute,UpdateRouteCommand command)
        {
            Console.WriteLine($"Updating route with ID: {idRoute}");
            var route = await routeRepository.FindByIdAsync(idRoute);
            if (route == null)
            {
                return null; // Route not found
            }
            var updatedRoute = new RouteAggregate(command);
            try
            {
                routeRepository.Update(updatedRoute);
                await unitOfWork.CompleteAsync();
                return updatedRoute;
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error updating route with ID {RouteId}.", command.IdRoute);
                return null; // Signal failure to the controller
            }
        }
        public async Task Handle(DeleteRouteCommand command)
        {
            var route = await routeRepository.FindByIdAsync(command.idRoute);
            if (route == null)
            {
                // logger?.LogWarning("Route with ID {RouteId} not found for deletion.", command.IdRoute);
                return; // Route not found, nothing to delete
            }
            try
            {
                routeRepository.Remove(route);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error deleting route with ID {RouteId}.", command.IdRoute);
                throw; // Re-throw the exception to be handled by the global exception handler
            }
        }
    }
}
