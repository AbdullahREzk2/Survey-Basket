namespace SurveyBasket.BLL.Features.Questions.Command.UpdateQuestion;
public record UpdateQuestionCommand(int pollId, int questionId, QuestionRequestDTO question) : IRequest<Result>;
