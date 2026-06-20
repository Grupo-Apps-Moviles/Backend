using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Domain.Model.Queries;
using Backend_Frock.Companies.Domain.Model.ValueObjects;
using Backend_Frock.Companies.Domain.Services;
using Backend_Frock.IAM.Application.OutboundServices;
using Backend_Frock.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Backend_Frock.Subscriptions.Domain.Model.Commands;
using Backend_Frock.Subscriptions.Domain.Model.Queries;
using Backend_Frock.Subscriptions.Domain.Service;
using Backend_Frock.Subscriptions.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Frock.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/subscriptions")]
[Authorize] // tu [Authorize] del IAM propio
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService queryService,
    ICompanyMembershipQueryService membershipQueryService,
    ITokenService tokenService) : ControllerBase
{
    // Lee el userId del JWT igual que hace RequestAuthorizationMiddleware
    private async Task<int?> GetUserId()
    {
        var token = Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();
        return await tokenService.ValidateToken(token ?? string.Empty);
    }

    // Resuelve la membresía del usuario actual (compañía + rol)
    private async Task<CompanyMembership?> GetMyMembership(int userId) =>
        await membershipQueryService.Handle(new GetMembershipByUserIdQuery(userId));

    // POST api/v1/subscriptions  — SOLO ADMIN
    [HttpPost]
    public async Task<IActionResult> CreateSubscription()
    {
        var userId = await GetUserId();
        if (userId is null)
            return Unauthorized(new { message = "Token inválido." });

        var membership = await GetMyMembership(userId.Value);
        if (membership is null)
            return Conflict(new { message = "No perteneces a ninguna empresa." });
        if (membership.MemberRole != MemberRole.Admin)
            return StatusCode(403, new { message = "Solo el administrador puede gestionar la suscripción." });

        var approvalUrl = await commandService
            .Handle(new CreateSubscriptionCommand(membership.CompanyId));

        if (approvalUrl is null)
            return Conflict(new { message = "La empresa ya tiene una suscripción activa." });

        return Ok(new CreateSubscriptionResource(approvalUrl));
    }

    // GET api/v1/subscriptions/me  — cualquier miembro ve el estado de SU empresa
    [HttpGet("me")]
    public async Task<IActionResult> GetMySubscription()
    {
        var userId = await GetUserId();
        if (userId is null)
            return Unauthorized();

        var membership = await GetMyMembership(userId.Value);
        if (membership is null)
            return Ok(new SubscriptionStatusResource(false, "None", null));

        var subscription = await queryService
            .Handle(new GetSubscriptionByCompanyIdQuery(membership.CompanyId));

        if (subscription is null)
            return Ok(new SubscriptionStatusResource(false, "None", null));

        return Ok(new SubscriptionStatusResource(
            subscription.IsActive,
            subscription.Status.ToString(),
            subscription.IsActive ? subscription.EndDate : null
        ));
    }

    // GET api/v1/subscriptions/premium-feature  — gating por compañía
    [HttpGet("premium-feature")]
    public async Task<IActionResult> PremiumFeature()
    {
        var userId = await GetUserId();
        if (userId is null)
            return Unauthorized();

        var membership = await GetMyMembership(userId.Value);
        if (membership is null)
            return StatusCode(403, new { message = "No perteneces a ninguna empresa." });

        var sub = await queryService
            .Handle(new GetSubscriptionByCompanyIdQuery(membership.CompanyId));

        if (sub is null || !sub.IsActive)
            return StatusCode(403, new { message = "Se requiere suscripción activa de la empresa." });

        return Ok(new { message = "Acceso premium concedido." });
    }
}
