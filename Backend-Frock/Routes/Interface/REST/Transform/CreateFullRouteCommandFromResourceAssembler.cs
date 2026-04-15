using Backend_Frock.Routes.Domain.Model.Commands;
using Backend_Frock.Routes.Interface.REST.Resources;
using Backend_Frock.Routes.Domain.Model.Entities;
namespace Backend_Frock.Routes.Interface.REST.Transform
{
    public class CreateFullRouteCommandFromResourceAssembler
    {
        public static CreateFullRouteCommand toCommandFromResource(CreateFullRouteResource resource) =>
            new CreateFullRouteCommand(
                resource.Price,
                resource.Duration,
                resource.Frequency,
                resource.StopsIds,
                resource.Schedules.Select(schedule => new Schedule(
                    schedule.StartTime,
                    schedule.EndTime,
                    schedule.DayOfWeek,
                    schedule.Enabled
                )).ToList()
            );
    }
}
