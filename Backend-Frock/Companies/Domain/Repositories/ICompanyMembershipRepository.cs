using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Companies.Domain.Repositories;

public interface ICompanyMembershipRepository : IBaseRepository<CompanyMembership>
{
    Task<CompanyMembership?> FindByUserIdAsync(int userId);
    Task<IEnumerable<CompanyMembership>> ListByCompanyIdAsync(int companyId);
    Task<int>  CountByCompanyIdAsync(int companyId);
    Task<bool> ExistsByCompanyAndUserAsync(int companyId, int userId);
}
