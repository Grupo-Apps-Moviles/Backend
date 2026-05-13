namespace Backend_Frock.Suscriptions.Infrastructure.ExternalServices
{
    public class PaypalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId = "TU_CLIENT_ID_BUSINESS";
        private readonly string _clientSecret = "TU_SECRET_BUSINESS";

        public PaypalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessToken()
        {
            var authToken = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") });
            var response = await _httpClient.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);

            // Aquí extraes el access_token del JSON de respuesta
            return "access_token_extraido";
        }
    }
}
