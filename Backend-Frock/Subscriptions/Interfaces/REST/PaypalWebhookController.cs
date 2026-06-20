using System.Text.Json;
using Backend_Frock.Subscriptions.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Frock.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/paypal/webhooks")]
public class PaypalWebhookController(
    ISubscriptionCommandService commandService,
    ILogger<PaypalWebhookController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> HandleWebhook([FromBody] JsonElement payload)
    {
        var eventType = payload
            .GetProperty("event_type").GetString() ?? string.Empty;
 
        var subscriptionId = payload
            .GetProperty("resource")
            .GetProperty("id").GetString() ?? string.Empty;
 
        logger.LogInformation(
            "PayPal webhook: {EventType} — suscripción {SubscriptionId}",
            eventType, subscriptionId);
 
        switch (eventType)
        {
            case "BILLING.SUBSCRIPTION.ACTIVATED":
                await commandService.HandleActivation(subscriptionId);
                break;
 
            case "BILLING.SUBSCRIPTION.CANCELLED":
            case "BILLING.SUBSCRIPTION.SUSPENDED":
                await commandService.HandleCancellation(subscriptionId);
                break;
 
            case "BILLING.SUBSCRIPTION.RENEWED":
            case "PAYMENT.SALE.COMPLETED":
                await commandService.HandleRenewal(subscriptionId);
                break;
 
            default:
                logger.LogDebug("Evento PayPal no manejado: {EventType}", eventType);
                break;
        }
 
        // Siempre 200 — si devolvemos otro código PayPal reintenta
        return Ok();
    }
}