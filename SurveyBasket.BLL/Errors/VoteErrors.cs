namespace SurveyBasket.BLL.Errors;
public static class VoteErrors
{
    public static Error UserAlreadyVoted =
        new Error("Vote.UserAlreadyVoted", "The user has already voted in this poll.", StatusCodes.Status409Conflict);
    public static Error InvalidQuestions =
        new Error("Vote.InvalidQuestions", "The provided questions are invalid for this poll.", StatusCodes.Status400BadRequest);
    public static Error VoteNotAdded =
        new Error("Vote.VoteNotAdded", "The vote could not be added due to an internal error.", StatusCodes.Status400BadRequest);
    public static Error DuplicateQuestionAnswer =
        new Error("Vote.DuplicateQuestionAnswer", "Duplicate answers found for the same question.", StatusCodes.Status409Conflict);
    public static Error InvalidAnswers =
        new Error("Vote.InvalidAnswers", "The provided answers are invalid for the given questions.", StatusCodes.Status409Conflict);
}
