using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Commands;
using Backend_Frock.Companies.Domain.Model.ValueObjects;
using Backend_Frock.Companies.Domain.Repositories;
using Backend_Frock.Companies.Domain.Services;
using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Subscriptions.Domain.Model.Queries;
using Backend_Frock.Subscriptions.Domain.Service;

namespace Backend_Frock.Companies.Application.Internal.CommandServices
{
    /// <summary>
    ///     Command service for the CompanyMembership aggregate.
    ///     Business rules are implemented per the Fase 1 spec (§5.1 / §10).
    ///     Exceptions are surfaced and mapped by the controller:
    ///       KeyNotFoundException        -> 404
    ///       InvalidOperationException   -> 409
    ///       UnauthorizedAccessException -> 403
    /// </summary>
    public class CompanyMembershipCommandService(
        ICompanyMembershipRepository membershipRepository,
        ICompanyRepository companyRepository,
        ISubscriptionQueryService subscriptionQueryService,
        IUnitOfWork unitOfWork) : ICompanyMembershipCommandService
    {
        public async Task<CompanyMembership?> Handle(JoinCompanyCommand command)
        {
            // 1) Resolver compañía por código.
            var company = await companyRepository.FindByInvitationCodeAsync(command.InvitationCode);
            if (company is null)
                throw new KeyNotFoundException("Código inválido.");

            // 2) El usuario no puede tener una membresía previa.
            var existing = await membershipRepository.FindByUserIdAsync(command.UserId);
            if (existing is not null)
                throw new InvalidOperationException("Ya perteneces a una empresa.");

            // 3) FASE 3: validación de capacidad según la suscripción de la compañía.
            //    Tope efectivo: suscripción activa => MaxMembers; sino => 1 (free).
            var subscription = await subscriptionQueryService
                .Handle(new GetSubscriptionByCompanyIdQuery(company.Id));
            var maxMembers = (subscription is { IsActive: true }) ? subscription.MaxMembers : 1;

            var currentCount = await membershipRepository.CountByCompanyIdAsync(company.Id);
            if (currentCount >= maxMembers)
                throw new InvalidOperationException(
                    "La empresa alcanzó su límite de miembros. Se requiere una suscripción activa para agregar más conductores.");

            // 4) Crear la membresía como Driver.
            var membership = new CompanyMembership(company.Id, command.UserId, MemberRole.Driver);
            await membershipRepository.AddAsync(membership);
            await unitOfWork.CompleteAsync();
            return membership;
        }

        public async Task<bool> Handle(LeaveCompanyCommand command)
        {
            var membership = await membershipRepository.FindByUserIdAsync(command.UserId);
            if (membership is null)
                throw new KeyNotFoundException("No perteneces a ninguna empresa.");

            if (membership.IsAdmin)
                throw new InvalidOperationException(
                    "El admin no puede abandonar la empresa; elimina la empresa o transfiere la administración.");

            membershipRepository.Remove(membership);
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> Handle(RemoveMemberCommand command)
        {
            // 1) El solicitante debe existir y ser Admin.
            var requester = await membershipRepository.FindByUserIdAsync(command.RequesterUserId);
            if (requester is null || !requester.IsAdmin)
                throw new UnauthorizedAccessException("Solo un administrador puede expulsar miembros.");

            // 2) El objetivo debe existir y pertenecer a la misma empresa.
            var target = await membershipRepository.FindByIdAsync(command.TargetMembershipId);
            if (target is null)
                throw new KeyNotFoundException("Miembro no encontrado.");
            if (target.CompanyId != requester.CompanyId)
                throw new UnauthorizedAccessException("El miembro no pertenece a tu empresa.");

            // 3) No auto-eliminación (usar Leave) ni eliminar a un Admin.
            if (target.UserId == requester.UserId)
                throw new InvalidOperationException("No puedes expulsarte a ti mismo; usa abandonar empresa.");
            if (target.IsAdmin)
                throw new InvalidOperationException("No se puede expulsar a un administrador.");

            membershipRepository.Remove(target);
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Company?> Handle(RegenerateInvitationCodeCommand command)
        {
            // 1) El solicitante debe ser Admin de esa misma empresa.
            var requester = await membershipRepository.FindByUserIdAsync(command.RequesterUserId);
            if (requester is null || !requester.IsAdmin || requester.CompanyId != command.CompanyId)
                throw new UnauthorizedAccessException("Solo un administrador puede regenerar el código.");

            // 2) Regenerar el código de la compañía.
            var company = await companyRepository.FindByIdAsync(command.CompanyId);
            if (company is null)
                throw new KeyNotFoundException("Empresa no encontrada.");

            company.RegenerateInvitationCode();
            companyRepository.Update(company);
            await unitOfWork.CompleteAsync();
            return company;
        }
    }
}
