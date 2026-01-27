namespace SurveyBasket.BLL.Contracts.Dashboard;
public record VotesPerDayResponse(
    DateOnly Date,
    int VoteCount
    );

