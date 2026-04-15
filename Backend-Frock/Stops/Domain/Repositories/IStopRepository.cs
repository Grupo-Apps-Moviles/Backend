using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Stops.Domain.Model.Aggregates;

namespace Backend_Frock.Stops.Domain.Repositories
{
    public interface IStopRepository : IBaseRepository<Stop>
    {
        Task<IEnumerable<Stop>> FindByFkIdCompanyAsync(int fkIdCompany);
        Task<IEnumerable<Stop>> FindByFkIdDistrictAsync(int fkIdDistrict);
        Task<Stop?> FindByNameAndFkIdDistrictAsync(string name, int fkIdDistrict);

        Task<Stop?> FindByNameAndFkIdCompanyAsync(string name, int fkIdCompany);
    }
}
