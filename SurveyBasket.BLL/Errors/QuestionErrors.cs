namespace SurveyBasket.BLL.Errors;
public static class QuestionErrors
{
    public static Error QuestionNotFound =
        new("Question.NotFound", "There's No Question", StatusCodes.Status404NotFound);
    public static Error QuestionCreationFailed =
        new("Question.CreationFailed", "Failed to create Question", StatusCodes.Status400BadRequest);
    public static Error QuestionAlreadyExists =
        new("Question.AlreadyExists", "Question with the same content already exists", StatusCodes.Status409Conflict);
    public static Error QuestionUpdateFailed =
        new("Question.UpdateFailed", "Failed to update Question", StatusCodes.Status400BadRequest);
    public static Error QuestionDeletionFailed =
        new("Question.DeletionFailed", "Failed to delete Question", StatusCodes.Status400BadRequest);
    public static Error QuestionActivationFailed =
        new("Question.ActivationFailed", "Failed to activate/deactivate Question", StatusCodes.Status400BadRequest);
}
