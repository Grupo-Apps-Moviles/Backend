using Backend_Frock.Stops.Domain.Model.DTOs;
using Backend_Frock.Stops.Domain.Services;
using System.Text.Json;

namespace Backend_Frock.Stops.Application.External
{
    public class GeoImportService: IGeoImportService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public GeoImportService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["GeoApi:BaseUrl"];
        }

        public async Task<IEnumerable<GeoResponseDto>> GetGeoFromApi()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var geoData = JsonSerializer.Deserialize<IEnumerable<GeoResponseDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return geoData ?? Enumerable.Empty<GeoResponseDto>();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Error fetching geographic data from external API", ex);
            }
        }
    }
}
