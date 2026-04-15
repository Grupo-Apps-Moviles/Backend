
namespace Backend_Frock.Routes.Interface.REST.Resources
{
    public record RouteAggregateResource
    (
        int id,
        double price,
        int frequency,
        int duration,
        List<StopInRoutesResource> stops,
        List<ScheduleResource> schedules
    );
}
