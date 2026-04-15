using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Commands;

namespace Backend_Frock.Companies.Domain.Services
{
    public interface ICompanyCommandService
    {
        Task<Company?> Handle(CreateCompanyCommand command);
        Task<Company?> Handle(UpdateCompanyCommand command);
        Task<Company?> Handle(DeleteCompanyCommand command);
    }
}
