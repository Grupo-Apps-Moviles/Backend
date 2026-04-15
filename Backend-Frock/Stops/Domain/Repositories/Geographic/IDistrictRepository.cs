using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;

namespace Backend_Frock.Stops.Domain.Repositories.Geographic
{
    public interface IDistrictRepository : IBaseStringRepository<District>
    {
        Task<IEnumerable<District>> FindByFkIdProvinceAsync(int fkIdProvince);
    }
}
