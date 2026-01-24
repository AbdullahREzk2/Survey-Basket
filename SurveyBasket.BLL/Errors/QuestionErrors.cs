namespace SurveyBasket.BLL.Errors;
public static class QuestionErrors
{
    public static Error QuestionNotFound =
        new("Question.NotFound", "There's No Question");
    public static Error QuestionCreationFailed =
        new("Question.CreationFailed", "Failed to create Question");
    public static Error QuestionAlreadyExists =
        new("Question.AlreadyExists", "Question with the same content already exists");
    public static Error QuestionUpdateFailed =
        new("Question.UpdateFailed", "Failed to update Question");
    public static Error QuestionDeletionFailed =
        new("Question.DeletionFailed", "Failed to delete Question");
}
