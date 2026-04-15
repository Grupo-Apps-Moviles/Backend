namespace Backend_Frock.Routes.Interface.REST.Resources
{
    public record CreateScheduleResource(
        string DayOfWeek,
        string StartTime,
        string EndTime,
        bool Enabled
    );
}
