namespace SurveyBasket.BLL.Features.Questions.Query.GetAvailableQuestion;
public record GetAvailableQuestionQuery(int pollId, string userId) : IRequest<Result<IEnumerable<QuestionResponseDTO>>>;
