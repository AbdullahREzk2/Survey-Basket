namespace SurveyBasket.BLL.Contracts.Dashboard;
public record VoteResponse(
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> selectedAnswers
    );
