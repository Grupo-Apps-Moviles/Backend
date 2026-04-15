using Backend_Frock.Stops.Domain.Model.Commands.Geographic;
using Backend_Frock.Stops.Interfaces.REST.Resources.Geographic;

namespace Backend_Frock.Stops.Interfaces.REST.Transform.Geographic
{
    public class CreateDistrictCommandFromResourceAssembler
    {
        public static CreateDistrictCommand ToCommandFromResource(CreateDistrictResource resource) =>
            new CreateDistrictCommand(
                resource.Id,
                resource.Name,
                resource.FkIdProvince
            );
    }
}
