using SurveyBasket.BLL.Features.ProfileImage.Command.DeleteImageByPublicId;
using SurveyBasket.BLL.Features.ProfileImage.Command.UploadImage;

namespace SurveyBasket.BLL.Features.Users.Command.UploadProfileImage;
public class UploadProfileImageCommandHandler(IUserRepository userRepository,IMediator mediator) : IRequestHandler<UploadProfileImageCommand, Result<string>>
{
    private readonly IUserRepository _userrepository = userRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<Result<string>> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _userrepository.FindByIdAsync(request.userId);
        if (user is null)
            return Result.Failure<string>(UserErrors.UserNotFound);

        if (!string.IsNullOrEmpty(user.ImageURL))
        {
            var publicId = GetPublicIdFromUrl(user.ImageURL);
            await _mediator.Send(new DeleteImageByPublicIdCommand(publicId));
        }

        var imageUrl = await _mediator.Send(new UploadImageCommand(request.image));
        if (imageUrl is null)
            return Result.Failure<string>(UserErrors.ImageUploadFailed);

        user.ImageURL = imageUrl;
        await _userrepository.UpdateAsync(user);

        return Result.Success(imageUrl);
    }

    private static string GetPublicIdFromUrl(string url)
    {
        var uri = new Uri(url);
        var segments = uri.AbsolutePath.Split('/');
        var uploadIndex = Array.IndexOf(segments, "upload");
        var publicIdSegments = segments.Skip(uploadIndex + 2);
        var publicId = string.Join("/", publicIdSegments);
        return Path.GetFileNameWithoutExtension(publicId) is var name
            ? publicId.Replace(Path.GetFileName(publicId), name)
            : publicId;
    }

}
