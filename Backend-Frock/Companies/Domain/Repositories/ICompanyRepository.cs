using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Companies.Domain.Model.Aggregates;


namespace Backend_Frock.Companies.Domain.Repositories
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<Company?> FindByNameAsync(string name);
        Task<Company?> FindByFkIdUserAsync(int FkUserId);
        Task<Company?> FindByInvitationCodeAsync(string invitationCode);
    }
}
