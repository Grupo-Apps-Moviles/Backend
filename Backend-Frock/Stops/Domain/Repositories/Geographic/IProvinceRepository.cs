using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;

namespace Backend_Frock.Stops.Domain.Repositories.Geographic
{
    public interface IProvinceRepository : IBaseStringRepository<Province>
    {
        Task<IEnumerable<Province>> FindByFkIdRegionAsync(int fkIdRegion);
    }
}
