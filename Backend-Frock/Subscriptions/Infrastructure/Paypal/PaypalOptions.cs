namespace Backend_Frock.Subscriptions.Infrastructure.Paypal;

public class PaypalOptions
{
    public const string SectionName = "PayPal";
    public string ClientId { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    /// <summary>
    /// Sandbox: https://api-m.sandbox.paypal.com
    /// Producción: https://api-m.paypal.com
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    /// <summary>ID del plan mensual — P-XXXXXXXXXXXX</summary>
    public string PlanId { get; set; } = string.Empty;
}