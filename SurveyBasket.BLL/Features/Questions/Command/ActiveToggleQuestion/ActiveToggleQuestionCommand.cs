namespace SurveyBasket.BLL.Features.Questions.Command.ActiveToggleQuestion;
public record ActiveToggleQuestionCommand(int pollId, int questionId) : IRequest<Result>; 
