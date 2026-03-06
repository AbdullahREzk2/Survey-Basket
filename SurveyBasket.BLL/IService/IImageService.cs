namespace SurveyBasket.BLL.IService;
public interface IImageService
{
    Task<string?> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task DeleteAsync(string publicId, CancellationToken cancellationToken = default);
}
