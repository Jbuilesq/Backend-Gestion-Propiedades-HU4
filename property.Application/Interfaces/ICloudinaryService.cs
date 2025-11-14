using property.Application.DTOs;

namespace property.Application.Interfaces;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(UploadFileDto file);
}