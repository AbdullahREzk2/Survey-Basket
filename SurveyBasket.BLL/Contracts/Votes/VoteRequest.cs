namespace SurveyBasket.BLL.Contracts.Votes;
public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
    );

