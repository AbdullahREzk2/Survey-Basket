namespace SurveyBasket.BLL.Errors;
public static class VoteErrors
{
    public static Error UserAlreadyVoted =
        new Error("Vote.UserAlreadyVoted", "The user has already voted in this poll.");
}
