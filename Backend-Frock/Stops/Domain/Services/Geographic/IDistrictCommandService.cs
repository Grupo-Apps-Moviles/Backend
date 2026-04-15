using Backend_Frock.Stops.Domain.Model.Commands.Geographic;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;

namespace Backend_Frock.Stops.Domain.Services.Geographic
{
    public interface IDistrictCommandService
    {
        Task<District?> Handle(CreateDistrictCommand command);
    }
}
