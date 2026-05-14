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
    ITokenService tokenService) : ControllerBase
{
    // Lee el userId del JWT igual que hace RequestAuthorizationMiddleware
    private async Task<int?> GetDriverId()
    {
        var token = Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();
        return await tokenService.ValidateToken(token ?? string.Empty);
    }
 
    // POST api/v1/subscriptions
    [HttpPost]
    public async Task<IActionResult> CreateSubscription()
    {
        var driverId = await GetDriverId();
        if (driverId is null)
            return Unauthorized(new { message = "Token inválido." });
 
        var approvalUrl = await commandService
            .Handle(new CreateSubscriptionCommand(driverId.Value));
 
        if (approvalUrl is null)
            return Conflict(new { message = "Ya tienes una suscripción activa." });
 
        return Ok(new CreateSubscriptionResource(approvalUrl));
    }
 
    // GET api/v1/subscriptions/me
    [HttpGet("me")]
    public async Task<IActionResult> GetMySubscription()
    {
        var driverId = await GetDriverId();
        if (driverId is null)
            return Unauthorized();
 
        var subscription = await queryService
            .Handle(new GetSubscriptionByDriverIdQuery(driverId.Value));
 
        if (subscription is null)
            return Ok(new SubscriptionStatusResource(false, "None", null));
 
        return Ok(new SubscriptionStatusResource(
            subscription.IsActive,
            subscription.Status.ToString(),
            subscription.IsActive ? subscription.EndDate : null
        ));
    }
 
    // GET api/v1/subscriptions/premium-feature
    [HttpGet("premium-feature")]
    public async Task<IActionResult> PremiumFeature()
    {
        var driverId = await GetDriverId();
        if (driverId is null)
            return Unauthorized();
 
        var sub = await queryService
            .Handle(new GetSubscriptionByDriverIdQuery(driverId.Value));
 
        if (sub is null || !sub.IsActive)
            return StatusCode(403, new { message = "Se requiere suscripción activa." });
 
        return Ok(new { message = "Acceso premium concedido." });
    }
}