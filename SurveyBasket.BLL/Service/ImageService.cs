using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SurveyBasket.BLL.Setting;

namespace SurveyBasket.BLL.Service;
public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IOptions<CloudinarySettings> options)
    {
        var settings = options.Value;
        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string?> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "survey-basket/profiles",
            Transformation = new Transformation().Width(500).Height(500).Crop("fill")
        };

        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        return result.Error is null ? result.SecureUrl.ToString() : null;
    }

    public async Task DeleteAsync(string publicId, CancellationToken cancellationToken = default)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}
