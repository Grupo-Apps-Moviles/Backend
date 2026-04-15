using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Interfaces.REST.Resources.Geographic;

namespace Backend_Frock.Stops.Interfaces.REST.Transform.Geographic
{
    public static class RegionResourceFromEntityAssembler
    {
        public static RegionResource ToResourceFromEntity(Region entity) =>
            new RegionResource(
                entity.Id,
                entity.Name
            );
    }
}
