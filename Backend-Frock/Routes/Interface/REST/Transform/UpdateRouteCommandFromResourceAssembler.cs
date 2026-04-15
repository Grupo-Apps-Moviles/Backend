using Backend_Frock.Routes.Domain.Model.Commands;
using Backend_Frock.Routes.Domain.Model.Entities;
using Backend_Frock.Routes.Interface.REST.Resources;

namespace Backend_Frock.Routes.Interface.REST.Transform
{
    public class UpdateRouteCommandFromResourceAssembler
    {
        public static UpdateRouteCommand toCommandFromResource(UpdateRouteResource resource)
        {
            return new UpdateRouteCommand
            (
                Price: resource.Price,
                Duration: resource.Duration,
                Frequency: resource.Frequency,
                StopsIds: resource.StopsIds,
                Schedules: resource.Schedules.Select(schedule => new Schedule
                (
                    startTime: schedule.startTime,
                    endTime: schedule.endTime,
                    dayOfWeek: schedule.dayOfWeek,
                    enabled: schedule.enabled
                )).ToList()
            );
        }
    }
}
