using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Interfaces.REST.Resources.Geographic;

namespace Backend_Frock.Stops.Interfaces.REST.Transform.Geographic
{
    public static class DistrictResourceFromEntityAssembler
    {
        public static DistrictResource ToResourceFromEntity(District entity) =>
            new DistrictResource(
                entity.Id,
                entity.Name,
                entity.FkIdProvince
            );
    }
}
