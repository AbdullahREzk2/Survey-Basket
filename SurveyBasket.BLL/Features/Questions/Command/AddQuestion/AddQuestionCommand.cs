namespace SurveyBasket.BLL.Features.Questions.Command.AddQuestion;
public record AddQuestionCommand(int pollId, QuestionRequestDTO question) : IRequest<Result<QuestionResponseDTO>>;
