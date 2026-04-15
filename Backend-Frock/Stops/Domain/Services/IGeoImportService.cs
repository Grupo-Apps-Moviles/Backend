using Backend_Frock.Stops.Domain.Model.DTOs;

namespace Backend_Frock.Stops.Domain.Services
{
    public interface IGeoImportService
    {
        Task<IEnumerable<GeoResponseDto>> GetGeoFromApi();
    }
}
