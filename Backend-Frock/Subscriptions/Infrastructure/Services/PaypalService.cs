using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Backend_Frock.Subscriptions.Domain.Service;
using Backend_Frock.Subscriptions.Infrastructure.Paypal;
using Microsoft.Extensions.Options;

namespace Backend_Frock.Subscriptions.Infrastructure.Services;

public class PaypalService(HttpClient httpClient, IOptions<PaypalOptions> options)
    : IPaypalService
{
    private readonly PaypalOptions _options = options.Value;

    // ─── OAuth2 ───────────────────────────────────────────────────────────

    private async Task<string> GetAccessTokenAsync()
    {
        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.Secret}"));

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_options.BaseUrl}/v1/oauth2/token");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Basic", credentials);

        request.Content = new StringContent(
            "grant_type=client_credentials",
            Encoding.UTF8,
            "application/x-www-form-urlencoded");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }

    // ─── Crear suscripción ────────────────────────────────────────────────

    public async Task<(string approvalUrl, string subscriptionId, string planId)>
        CreateSubscriptionAsync(int driverId)
    {
        var token = await GetAccessTokenAsync();

        var body = new
        {
            plan_id = _options.PlanId,
            custom_id = driverId.ToString(),
            application_context = new
            {
                brand_name = "WayPass",
                locale = "es-PE",
                shipping_preference = "NO_SHIPPING",
                user_action = "SUBSCRIBE_NOW",
                return_url = "waypass://paypal/success",
                cancel_url = "waypass://paypal/cancel"
            }
        };

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_options.BaseUrl}/v1/billing/subscriptions");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        request.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var subscriptionId = doc.RootElement
            .GetProperty("id").GetString()!;

        var approvalUrl = doc.RootElement
            .GetProperty("links")
            .EnumerateArray()
            .First(x => x.GetProperty("rel").GetString() == "approve")
            .GetProperty("href")
            .GetString()!;

        return (approvalUrl, subscriptionId, _options.PlanId);
    }

    // ─── Validar suscripción ──────────────────────────────────────────────

    public async Task<bool> ValidateSubscriptionAsync(string paypalSubscriptionId)
    {
        var token = await GetAccessTokenAsync();

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_options.BaseUrl}/v1/billing/subscriptions/{paypalSubscriptionId}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("status").GetString() == "ACTIVE";
    }
}