using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Repositories;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend_Frock.Companies.Infrastructure.Repositories
{
    public class CompanyMembershipRepository(AppDbContext context)
        : BaseRepository<CompanyMembership>(context), ICompanyMembershipRepository
    {
        public Task<CompanyMembership?> FindByUserIdAsync(int userId) =>
            Context.Set<CompanyMembership>().FirstOrDefaultAsync(m => m.UserId == userId);

        public async Task<IEnumerable<CompanyMembership>> ListByCompanyIdAsync(int companyId) =>
            await Context.Set<CompanyMembership>().Where(m => m.CompanyId == companyId).ToListAsync();

        public Task<int> CountByCompanyIdAsync(int companyId) =>
            Context.Set<CompanyMembership>().CountAsync(m => m.CompanyId == companyId);

        public Task<bool> ExistsByCompanyAndUserAsync(int companyId, int userId) =>
            Context.Set<CompanyMembership>().AnyAsync(m => m.CompanyId == companyId && m.UserId == userId);
    }
}
