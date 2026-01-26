namespace SurveyBasket.BLL.Contracts.Votes;
public record VoteAnswerRequest(
    int QuestionId,
    int AnswerId
    );

