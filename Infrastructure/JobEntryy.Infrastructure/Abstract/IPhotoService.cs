using Microsoft.AspNetCore.Http;

namespace JobEntryy.Infrastructure.Abstract;

public interface IPhotoService
{
    Task<string> SavePhotoAsync(IFormFile Photo, string folder);
    void DeletePhoto(string path);
    Task<(bool, string)> PhotoChechkValidatorAsync(IFormFile photo, bool IsAllowNull, bool IsEqual256Kb);
}
