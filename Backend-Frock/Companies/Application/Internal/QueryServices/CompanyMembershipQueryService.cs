using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Queries;
using Backend_Frock.Companies.Domain.Repositories;
using Backend_Frock.Companies.Domain.Services;

namespace Backend_Frock.Companies.Application.Internal.QueryServices
{
    public class CompanyMembershipQueryService(ICompanyMembershipRepository membershipRepository)
        : ICompanyMembershipQueryService
    {
        public async Task<CompanyMembership?> Handle(GetMembershipByUserIdQuery query)
        {
            return await membershipRepository.FindByUserIdAsync(query.UserId);
        }

        public async Task<IEnumerable<CompanyMembership>> Handle(GetMembersByCompanyIdQuery query)
        {
            return await membershipRepository.ListByCompanyIdAsync(query.CompanyId);
        }

        public async Task<int> Handle(GetMemberCountByCompanyIdQuery query)
        {
            return await membershipRepository.CountByCompanyIdAsync(query.CompanyId);
        }
    }
}
