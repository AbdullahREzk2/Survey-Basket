namespace SurveyBasket.BLL.Errors;
public static class PollErrors
{
    public static Error PollNotFound = 
        new("Poll.NotFound", "The poll was not found.");

    public static Error PollAlreadyExists = 
        new("Poll.AlreadyExists", "A poll with the same title already exists.");

    public static Error PollCreationFailed =
        new("Poll.CreationFailed", "Failed to create the poll.");

    public static Error PollUPdateFailed =
        new("Poll.UpdateFailed", "Failed to Update the poll.");

    public static Error PollDeletionFailed =
        new("Poll.DeletionFailed", "Failed to delete the poll.");

    public static Error PollPublicationToggleFailed =
                new("Poll.PublicationToggleFailed", "Failed to toggle the publication status of the poll.");
}
