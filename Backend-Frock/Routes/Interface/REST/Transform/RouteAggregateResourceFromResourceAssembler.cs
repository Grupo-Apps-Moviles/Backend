using Backend_Frock.Routes.Interface.REST.Resources;
using Backend_Frock.Routes.Domain.Model.Aggregates;
namespace Backend_Frock.Routes.Interface.REST.Transform
{
    public class RouteAggregateResourceFromResourceAssembler
    {
        public static RouteAggregateResource ToResourceFromEntity(RouteAggregate routeAggregate) =>
            new RouteAggregateResource(
                routeAggregate.Id,
                routeAggregate.Price,
                routeAggregate.Frequency,
                routeAggregate.Duration,
                routeAggregate.Stops.Select(stop => new StopInRoutesResource(stop.Id, stop.Stop.Name, stop.Stop.GoogleMapsUrl, stop.Stop.ImageUrl, stop.Stop.Address, stop.Stop.FkIdCompany, stop.Stop.FkIdDistrict)).ToList(),
                routeAggregate.Schedules.Select(schedule => new ScheduleResource( schedule.StartTime, schedule.EndTime, schedule.DayOfWeek, schedule.Enabled)).ToList()
            );
   
    }
}
