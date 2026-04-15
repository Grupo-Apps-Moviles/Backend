using Backend_Frock.Routes.Domain.Model.Entities;

namespace Backend_Frock.Routes.Domain.Model.Commands
{
    public record CreateFullRouteCommand(
        double Price,
        int Duration,
        int Frequency,
        List<int> StopsIds,
        List<Schedule> Schedules
    );
}
