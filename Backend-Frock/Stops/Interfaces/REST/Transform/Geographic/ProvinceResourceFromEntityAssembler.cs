using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Interfaces.REST.Resources.Geographic;

namespace Backend_Frock.Stops.Interfaces.REST.Transform.Geographic
{
    public static class ProvinceResourceFromEntityAssembler
    {
        public static ProvinceResource ToResourceFromEntity(Province entity) =>
            new ProvinceResource(
                entity.Id,
                entity.Name,
                entity.FkIdRegion
            );
    }
}
