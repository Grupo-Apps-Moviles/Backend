using Backend_Frock.Stops.Domain.Model.Commands.Geographic;
using Backend_Frock.Stops.Interfaces.REST.Resources.Geographic;

namespace Backend_Frock.Stops.Interfaces.REST.Transform.Geographic
{
    public class CreateProvinceCommandFromResourceAssembler
    {
        public static CreateProvinceCommand ToCommandFromResource(CreateProvinceResource resource) =>
            new CreateProvinceCommand(
                resource.Id,
                resource.Name,
                resource.FkIdRegion
            );
    }
}
