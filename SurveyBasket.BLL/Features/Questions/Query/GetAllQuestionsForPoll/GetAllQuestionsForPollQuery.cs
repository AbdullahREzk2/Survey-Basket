namespace SurveyBasket.BLL.Features.Questions.Query.GetAllQuestionsForPoll;
public record GetAllQuestionsForPollQuery(int pollId, RequestFilters filters) : IRequest<Result<PaginatedList<QuestionResponseDTO>>>;
