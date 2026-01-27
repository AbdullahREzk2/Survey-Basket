namespace SurveyBasket.BLL.Errors;
public static class PollErrors
{
    public static Error PollNotFound = 
        new("Poll.NotFound", "The poll was not found.", StatusCodes.Status404NotFound);

    public static Error PollAlreadyExists = 
        new("Poll.AlreadyExists", "A poll with the same title already exists.",StatusCodes.Status409Conflict);

    public static Error PollCreationFailed =
        new("Poll.CreationFailed", "Failed to create the poll.", StatusCodes.Status400BadRequest);

    public static Error PollUPdateFailed =
        new("Poll.UpdateFailed", "Failed to Update the poll.", StatusCodes.Status400BadRequest);

    public static Error PollDeletionFailed =
        new("Poll.DeletionFailed", "Failed to delete the poll.", StatusCodes.Status400BadRequest);

    public static Error PollPublicationToggleFailed =
                new("Poll.PublicationToggleFailed", "Failed to toggle the publication status of the poll.", StatusCodes.Status400BadRequest);
}
