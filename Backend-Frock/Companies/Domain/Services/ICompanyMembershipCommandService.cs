using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Commands;

namespace Backend_Frock.Companies.Domain.Services
{
    public interface ICompanyMembershipCommandService
    {
        Task<CompanyMembership?> Handle(JoinCompanyCommand command);
        Task<bool>              Handle(LeaveCompanyCommand command);
        Task<bool>              Handle(RemoveMemberCommand command);
        Task<Company?>          Handle(RegenerateInvitationCodeCommand command);
    }
}
