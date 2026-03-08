namespace SurveyBasket.BLL.Features.Questions.Query.GetQuestionById;
public record GetQuestionByIdQuery(int pollId, int questionId) : IRequest<Result<QuestionResponseDTO>>;
