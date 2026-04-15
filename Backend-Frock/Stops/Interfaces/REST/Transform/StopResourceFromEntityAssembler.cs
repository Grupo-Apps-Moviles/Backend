using Backend_Frock.Stops.Domain.Model.Aggregates;
using Backend_Frock.Stops.Interfaces.REST.Resources;

namespace Backend_Frock.Stops.Interfaces.REST.Transform
{
    public static class StopResourceFromEntityAssembler
    {
        /// <summary>
        /// Assembles a StopResource from a Stop. 
        /// </summary>
        /// <param name="entity">The Stop entity</param>
        /// <returns>
        /// A StopResource assembled from the Stop
        /// </returns>
        public static StopResource ToResourceFromEntity(Stop entity) =>
            new StopResource(
                entity.Id,
                entity.Name,
                entity.GoogleMapsUrl,
                entity.ImageUrl,
                entity.Phone,
                entity.FkIdCompany,
                entity.Address,
                entity.Reference,
                entity.FkIdDistrict
            );
    }
}
