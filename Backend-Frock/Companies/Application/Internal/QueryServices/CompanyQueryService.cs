using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Queries;
using Backend_Frock.Companies.Domain.Repositories;
using Backend_Frock.Companies.Domain.Services;

namespace Backend_Frock.Companies.Application.Internal.QueryServices
{
    public class CompanyQueryService(ICompanyRepository companyRepository) : ICompanyQueryService
    {
        public async Task<IEnumerable<Company>> Handle(GetAllCompaniesQuery query)
        {
            return await companyRepository.ListAsync();
        }
        public async Task<Company?> Handle(GetCompanyByIdQuery query)
        {
            return await companyRepository.FindByIdAsync(query.Id);
        }
        public async Task<Company?> Handle(GetCompanyByNameQuery query)
        {
            return await companyRepository.FindByNameAsync(query.Name);
        }

        public async Task<Company?> Handle(GetCompanyByFkIdUserQuery query)
        {
            return await companyRepository.FindByFkIdUserAsync(query.FkIdUser);
        }
    }
}
