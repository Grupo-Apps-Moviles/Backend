using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Commands;
using Backend_Frock.Companies.Domain.Model.ValueObjects;
using Backend_Frock.Companies.Domain.Repositories;
using Backend_Frock.Companies.Domain.Services;

namespace Backend_Frock.Companies.Application.Internal.CommandServices
{
    public class CompanyCommandService(
        ICompanyRepository companyRepository,
        ICompanyMembershipRepository membershipRepository,
        IUnitOfWork unitOfWork) : ICompanyCommandService
    {
        public async Task<Company?> Handle(CreateCompanyCommand command)
        {
            var existingCompany = await companyRepository.FindByNameAsync(command.Name);
            if (existingCompany != null)
            {
                throw new Exception($"Company with name '{command.Name}' already exists.");
            }
            var newCompany = new Company(command);
            try
            {
                await companyRepository.AddAsync(newCompany);
                await unitOfWork.CompleteAsync();          // 1er save -> company.Id poblado

                // El creador queda como Admin de su propia compañía.
                var adminMembership = new CompanyMembership(
                    newCompany.Id, command.FkIdUser, MemberRole.Admin);
                await membershipRepository.AddAsync(adminMembership);
                await unitOfWork.CompleteAsync();          // 2do save

                return newCompany;
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error creating company with name {CompanyName}.", command.Name);
                return null; // Signal failure to the controller
            }
        }
        public async Task<Company?> Handle(UpdateCompanyCommand command)
        {
            var companyToUpdate = await companyRepository.FindByIdAsync(command.Id);
            if (companyToUpdate == null)
            {
                return null; // Company not found
            }

            // Apply changes from the command to the fetched entity
            companyToUpdate.Name = command.Name;
            companyToUpdate.LogoUrl = command.LogoUrl;
            companyToUpdate.FkIdUser = command.FkIdUser;

            try
            {
                companyRepository.Update(companyToUpdate); // Update the fetched and modified entity
                await unitOfWork.CompleteAsync();
                return companyToUpdate; // Return the updated entity
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error updating company with ID {CompanyId}.", command.Id);
                return null; // Signal failure to the controller
            }
        }

        public async Task<Company?> Handle(DeleteCompanyCommand command)
        {
            var companyToDelete = await companyRepository.FindByIdAsync(command.Id);
            if (companyToDelete == null)
            {
                return null; // Company not found
            }
            try
            {
                companyRepository.Remove(companyToDelete); // Delete the fetched entity
                await unitOfWork.CompleteAsync();
                return companyToDelete; // Return the deleted entity
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error deleting company with ID {CompanyId}.", command.Id);
                return null; // Signal failure to the controller
            }
        }

    }
}
