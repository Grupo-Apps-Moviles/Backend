using Backend_Frock.Routes.Domain.Model.Entities;

namespace Backend_Frock.Routes.Domain.Model.Commands
{
    public record UpdateRouteCommand
    (
        double Price,
        int Duration, // in minutes
        int Frequency, // in minutes
        List<int> StopsIds,
        List<Schedule> Schedules
    );    
}
