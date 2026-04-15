namespace Backend_Frock.Shared.Domain.Services;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, string folder = "companies");
    Task<bool> DeleteImageAsync(string publicId);
}