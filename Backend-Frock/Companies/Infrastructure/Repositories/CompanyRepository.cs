using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Repositories;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend_Frock.Companies.Infrastructure.Repositories
{
    public class CompanyRepository(AppDbContext context) : BaseRepository<Company>(context), ICompanyRepository
    {
        public async Task<Company?> FindByNameAsync(string name)
        {
            return await context.Set<Company>()
                .FirstOrDefaultAsync(f => f.Name == name);
        }

        public async Task<Company?> FindByFkIdUserAsync(int fkIdUser)
        {
            return await context.Set<Company>().FirstOrDefaultAsync(c => c.FkIdUser == fkIdUser);
        }

        public Task<Company?> FindByInvitationCodeAsync(string invitationCode)
        {
            return context.Set<Company>().FirstOrDefaultAsync(c => c.InvitationCode == invitationCode);
        }
    }
}