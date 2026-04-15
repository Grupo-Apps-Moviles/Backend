using Backend_Frock.Companies.Domain.Model.Commands;
using Backend_Frock.Companies.Interfaces.REST.Resources;

namespace Backend_Frock.Companies.Interfaces.REST.Transform
{
    public class DeleteCompanyCommandFromResourceAssembler
    {
        public static DeleteCompanyCommand ToCommandFromResource(DeleteCompanyResource resource)
        {
            return new DeleteCompanyCommand(resource.Id);
        }
    }
}
