namespace SurveyBasket.BLL.Errors;
public static class DashboardErrors
{
    public static readonly Error VotesPerDayNotFound = 
        new(Code: "Dashboard.VotesPerDayNotFound",Message: "Votes per day data not found for the specified poll.",StatusCodes.Status404NotFound);
    public static readonly Error QuestionVotesNotFound = 
        new(Code: "Dashboard.QuestionVotesNotFound",Message: "Question votes data not found for the specified poll.",StatusCodes.Status404NotFound);
}
