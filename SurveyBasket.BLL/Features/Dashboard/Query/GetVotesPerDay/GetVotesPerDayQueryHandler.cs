namespace SurveyBasket.BLL.Features.Dashboard.Query.GetVotesPerDay;
public class GetVotesPerDayQueryHandler(IPollRepository pollRepository,IDashboardRepository dashboardRepository) : IRequestHandler<GetVotesPerDayQuery, Result<IEnumerable<VotesPerDayResponse>>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IDashboardRepository _dashboardrepository = dashboardRepository;

    public async Task<Result<IEnumerable<VotesPerDayResponse>>> Handle(GetVotesPerDayQuery request, CancellationToken cancellationToken)
    {

        var Poll = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);
        if (Poll is null)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

        var votesPerDayDTOs = await _dashboardrepository.GetVotesPerDayAsync(request.pollId, cancellationToken);

        var VotesPerDay = votesPerDayDTOs.Adapt<IEnumerable<VotesPerDayResponse>>();

        if (VotesPerDay is null)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(DashboardErrors.VotesPerDayNotFound);

        return Result.Success(VotesPerDay);
    }
}
