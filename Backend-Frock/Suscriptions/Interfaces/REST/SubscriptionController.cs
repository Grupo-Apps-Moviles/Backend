using Backend_Frock.Suscriptions.Application.Internal.CommandServices;

[ApiController]
[Route("api/v1/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly SubscriptionCommandService _commandService;

    public SubscriptionsController(SubscriptionCommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionCommand command)
    {
        var result = await _commandService.Execute(command);
        return Ok(result);
    }

    // IMPORTANTE: Endpoint para el Webhook de PayPal
    [HttpPost("webhook")]
    public async Task<IActionResult> PaypalWebhook()
    {
        // Leer el body de PayPal y actualizar estado en MySQL a ACTIVE
        return Ok();
    }
}