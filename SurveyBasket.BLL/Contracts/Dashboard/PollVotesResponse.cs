namespace SurveyBasket.BLL.Contracts.Dashboard;
public record PollVotesResponse(
    string Title,
    IEnumerable<VoteResponse> Votes
    );
