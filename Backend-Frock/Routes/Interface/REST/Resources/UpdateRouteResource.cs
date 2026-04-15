namespace Backend_Frock.Routes.Interface.REST.Resources
{
    public record UpdateRouteResource
    (
        double Price,
        int Duration,           // in minutes
        int Frequency,          // in minutes
        List<int> StopsIds,
        List<ScheduleResource> Schedules
    );
}
