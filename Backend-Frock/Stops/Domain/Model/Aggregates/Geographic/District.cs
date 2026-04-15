using Backend_Frock.Stops.Domain.Model.Commands.Geographic;

namespace Backend_Frock.Stops.Domain.Model.Aggregates.Geographic
{
    public class District
    {
        public int Id { get; }
        public string Name { get; set; }
        public int FkIdProvince { get; set; }
        protected District()
        {
            Id = 0;
            Name = string.Empty;
            FkIdProvince = 0;
        }
        public District(CreateDistrictCommand command)
        {
            Id = command.Id;
            Name = command.Name;
            FkIdProvince = command.FkIdProvince;
        }
    }
}
