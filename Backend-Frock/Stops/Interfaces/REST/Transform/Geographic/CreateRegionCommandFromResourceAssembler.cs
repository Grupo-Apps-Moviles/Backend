using Backend_Frock.Stops.Domain.Model.Commands.Geographic;
using Backend_Frock.Stops.Interfaces.REST.Resources.Geographic;

namespace Backend_Frock.Stops.Interfaces.REST.Transform.Geographic
{
    public class CreateRegionCommandFromResourceAssembler
    {
        public static CreateRegionCommand ToCommandFromResource(CreateRegionResource resource) =>
            new CreateRegionCommand(
                resource.Id,
                resource.Name
                );
    }
}
