using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Queries;

namespace Backend_Frock.Companies.Domain.Services
{
    public interface ICompanyMembershipQueryService
    {
        Task<CompanyMembership?>             Handle(GetMembershipByUserIdQuery query);
        Task<IEnumerable<CompanyMembership>> Handle(GetMembersByCompanyIdQuery query);
        Task<int>                           Handle(GetMemberCountByCompanyIdQuery query);
    }
}
