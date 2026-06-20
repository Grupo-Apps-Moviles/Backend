using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Queries;

namespace Backend_Frock.Companies.Domain.Services
{
    public interface ICompanyQueryService
    {
        Task<IEnumerable<Company>> Handle(GetAllCompaniesQuery query);

        Task<Company?> Handle(GetCompanyByIdQuery query);

        Task<Company?> Handle(GetCompanyByNameQuery query);

        Task<Company?> Handle(GetCompanyByFkIdUserQuery query);

        Task<Company?> Handle(GetCompanyByInvitationCodeQuery query);

    }
}
