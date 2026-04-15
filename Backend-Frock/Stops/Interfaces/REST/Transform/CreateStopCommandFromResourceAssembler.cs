using Backend_Frock.Stops.Domain.Model.Commands;
using Backend_Frock.Stops.Interfaces.REST.Resources;

namespace Backend_Frock.Stops.Interfaces.REST.Transform
{
    /// <summary>
    /// Assembles a CreateStopCommand from a CreateStopResource. 
    /// </summary>
    /// <param name="resource">The CreateStopResource resource</param>
    /// <returns>
    /// A CreateStopCommand assembled from the CreateStopResource
    /// </returns>
    public class CreateStopCommandFromResourceAssembler
    {
        public static CreateStopCommand ToCommandFromResource(CreateStopResource resource) =>
            new CreateStopCommand(
            resource.Name,
            resource.GoogleMapsUrl,
            resource.ImageUrl,
            resource.Phone,
            resource.FkIdCompany,
            resource.Address,
            resource.Reference,
            resource.FkIdDistrict
                );
    }
}
