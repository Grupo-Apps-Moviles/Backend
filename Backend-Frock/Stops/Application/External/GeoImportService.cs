using Backend_Frock.Stops.Domain.Model.DTOs;
using Backend_Frock.Stops.Domain.Services;
using System.Text.Json;

namespace Backend_Frock.Stops.Application.External
{
    public class GeoImportService: IGeoImportService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeoImportService> _logger;
        private readonly string _apiUrl;

        // Esperas crecientes entre intentos. Son 4 esperas entre 5 intentos y suman
        // 40s, suficiente para tolerar el "cold start" del servicio gratuito de Railway,
        // que la primera vez puede tardar varios segundos en despertar y responder.
        private static readonly TimeSpan[] RetryDelays =
        {
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(8),
            TimeSpan.FromSeconds(12),
            TimeSpan.FromSeconds(15)
        };

        public GeoImportService(HttpClient httpClient, IConfiguration configuration, ILogger<GeoImportService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiUrl = configuration["GeoApi:BaseUrl"]
                      ?? throw new InvalidOperationException("GeoApi:BaseUrl no está configurado en appsettings.");
        }

        public async Task<IEnumerable<GeoResponseDto>> GetGeoFromApi()
        {
            var totalAttempts = RetryDelays.Length + 1;

            for (var attempt = 1; attempt <= totalAttempts; attempt++)
            {
                try
                {
                    _logger.LogInformation(
                        "Solicitando datos geográficos a {Url} (intento {Attempt}/{Total})...",
                        _apiUrl, attempt, totalAttempts);

                    var response = await _httpClient.GetAsync(_apiUrl);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var geoData = JsonSerializer.Deserialize<IEnumerable<GeoResponseDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })?.ToList() ?? new List<GeoResponseDto>();

                    if (geoData.Count == 0)
                    {
                        // 200 OK pero sin datos: lo tratamos como transitorio y reintentamos.
                        _logger.LogWarning(
                            "La API geográfica respondió sin datos en el intento {Attempt}/{Total}.",
                            attempt, totalAttempts);
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Datos geográficos obtenidos: {Count} registros en el intento {Attempt}.",
                            geoData.Count, attempt);
                        return geoData;
                    }
                }
                catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
                {
                    // Fallos transitorios típicos del cold start (502/503) o timeouts: reintentamos.
                    _logger.LogWarning(ex,
                        "Fallo transitorio al obtener datos geográficos (intento {Attempt}/{Total}): {Message}",
                        attempt, totalAttempts, ex.Message);
                }

                if (attempt < totalAttempts)
                {
                    var delay = RetryDelays[attempt - 1];
                    _logger.LogInformation("Reintentando en {Seconds}s...", delay.TotalSeconds);
                    await Task.Delay(delay);
                }
            }

            // Agotados todos los intentos: NO se traga el error, se propaga para que el llamador lo loguee.
            throw new Exception(
                $"No se pudieron obtener datos geográficos de la API externa ({_apiUrl}) tras {totalAttempts} intentos.");
        }
    }
}
