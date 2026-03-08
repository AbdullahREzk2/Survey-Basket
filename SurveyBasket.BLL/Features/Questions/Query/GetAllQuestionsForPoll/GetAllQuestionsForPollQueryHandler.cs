namespace SurveyBasket.BLL.Features.Questions.Query.GetAllQuestionsForPoll;
public class GetAllQuestionsForPollQueryHandler(IPollRepository pollRepository,IQuestionRepository questionRepository) : IRequestHandler<GetAllQuestionsForPollQuery, Result<PaginatedList<QuestionResponseDTO>>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IQuestionRepository _questionrepository = questionRepository;

    public async Task<Result<PaginatedList<QuestionResponseDTO>>> Handle(GetAllQuestionsForPollQuery request, CancellationToken cancellationToken)
    {
        var isPollExist = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);

        if (isPollExist == null)
            return Result.Failure<PaginatedList<QuestionResponseDTO>>(PollErrors.PollNotFound);

        var query = _questionrepository.GetAllQuestionsForPollAsync(request.pollId, request.filters.SearchValue!, request.filters.SortColumn!, request.filters.SortDirection!);

        if (!query.Any())
            return Result.Failure<PaginatedList<QuestionResponseDTO>>(
                QuestionErrors.QuestionNotFound);

        var projectedQuery = query.ProjectToType<QuestionResponseDTO>();

        var result = await PaginatedList<QuestionResponseDTO>.CreateAsync(projectedQuery,request.filters.PageNumber, request.filters.PageSize, cancellationToken);

        return Result.Success(result);
    }
}
