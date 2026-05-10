using Backend_Frock.Routes.Domain.Model.Commands;
using Backend_Frock.Routes.Domain.Model.Entities;

namespace Backend_Frock.Routes.Domain.Model.Aggregates
{
    public class RouteAggregate
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Duration { get; set; } // in minutes
        public int Frequency { get; set; } // in minutes
        public List<Schedule> Schedules = new();
        public List<RoutesStops> Stops = new();
        public RouteAggregate(double Price, int Duration, int Frequency)
        {
            this.Price = Price;
            this.Duration = Duration;
            this.Frequency = Frequency;
        }
        // 3) Método para añadir un horario
        public void AddSchedule(string start, string end, string dayOfWeek, bool enabled)
        {
            Schedules.Add(new Schedule( start, end, dayOfWeek, enabled));
        }
        public RouteAggregate(CreateFullRouteCommand cm)
        {
            this.Price = cm.Price;
            this.Duration = cm.Duration;
            this.Frequency = cm.Frequency;
            foreach (var stopId in cm.StopsIds)
            {
                // Assuming RoutesStops is a value object that holds the stop ID
                var routeStop = new RoutesStops(stopId);
                // Here you would typically add this to a collection of stops in the Route aggregate
                this.Stops.Add(routeStop);
            }
            foreach (var schedule in cm.Schedules)
            {
                this.AddSchedule(schedule.StartTime, schedule.EndTime, schedule.DayOfWeek, schedule.Enabled);
            }
        }
        public void UpdateInformation(UpdateRouteCommand cm)
        {
            this.Price = cm.Price;
            this.Duration = cm.Duration;
            this.Frequency = cm.Frequency;
            this.Stops.Clear();
            foreach (var stopId in cm.StopsIds)
            {
                this.Stops.Add(new RoutesStops(stopId));
            }

            this.Schedules.Clear();
            foreach (var schedule in cm.Schedules)
            {
                this.AddSchedule(schedule.StartTime, schedule.EndTime, schedule.DayOfWeek, schedule.Enabled);
            }
        }
    }
}
