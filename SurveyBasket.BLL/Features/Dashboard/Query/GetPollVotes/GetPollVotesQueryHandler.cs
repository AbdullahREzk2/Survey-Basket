namespace SurveyBasket.BLL.Features.Dashboard.Query.GetPollVotes;
public class GetPollVotesQueryHandler(IDashboardRepository dashboardRepository) : IRequestHandler<GetPollVotesQuery, Result<PollVotesResponse>>
{
    private readonly IDashboardRepository _dashboardrepository = dashboardRepository;

    public async Task<Result<PollVotesResponse>> Handle(GetPollVotesQuery request, CancellationToken cancellationToken)
    {

        var pollTitle = await _dashboardrepository.GetPollTitleAsync(request.pollId, cancellationToken);
        var voterDetails = await _dashboardrepository.GetVoterDetailsAsync(request.pollId, cancellationToken);
        var questionAnswers = await _dashboardrepository.GetQuestionAnswersAsync(request.pollId, cancellationToken);

        var votes = new PollVotesResponse(
            Title: pollTitle ?? "Unknown Poll",
            Votes: voterDetails.Select(vd => new VoteResponse(
                VoterName: vd.VoterName,
                VoteDate: vd.VoteDate,
                selectedAnswers: questionAnswers
                    .Where(qa => qa.VoteId == vd.VoteId)
                    .Select(qa => new QuestionAnswerResponse(
                        qa.QuestionContent,
                        qa.AnswerContent
                    ))
            ))
        );
        return votes is null
            ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
            : Result.Success(votes);
    }
}
