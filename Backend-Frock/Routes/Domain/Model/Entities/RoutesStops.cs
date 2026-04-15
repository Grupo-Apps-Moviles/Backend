using Backend_Frock.Routes.Domain.Model.Aggregates;
using Backend_Frock.Stops.Domain.Model.Aggregates;
namespace Backend_Frock.Routes.Domain.Model.Entities
{
    public class RoutesStops
    {
        public int Id { get; set; } // Unique identifier for the RoutesStops entity
        public int FkStopId { get; set; } // Foreign key to Route
        public int FKRouteId { get; set; } // Foreign key to Stop
        public RouteAggregate Route { get; set; } // Navigation property to Route
        public Stop Stop { get; set; } // Navigation property to Stop
        public RoutesStops(int FkStopId)
        {
            this.FkStopId = FkStopId;
        }
        public RoutesStops(int stopId, string name, string address, int fkCompanyId, int fkDistrictId)
        {
            this.Stop = new Stop(stopId, name, address, fkCompanyId, fkDistrictId);
        }
    }
}
