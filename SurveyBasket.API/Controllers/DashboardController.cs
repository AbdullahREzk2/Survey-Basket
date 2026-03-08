using SurveyBasket.BLL.Features.Dashboard.Query.GetPollVotes;
using SurveyBasket.BLL.Features.Dashboard.Query.GetQuestionVotes;
using SurveyBasket.BLL.Features.Dashboard.Query.GetVotesPerDay;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[HasPermission(Permissions.Results)]
public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;



    #region Dashboard-statics
    [HttpGet("get-Dashboard-statics/{pollId}")]
    public async Task<IActionResult> DashboardStatics(int pollId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPollVotesQuery(pollId), cancellationToken);
        return result.IsSuccess
             ? Ok(result.Value)
             : result.ToProblem();
    }
    #endregion

    #region getVotesPerDay
    [HttpGet("get-votes-per-day/{pollId}")]
    public async Task<IActionResult> GetVotesPerDay(int pollId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVotesPerDayQuery(pollId),cancellationToken);

        return result.IsSuccess
             ? Ok(result.Value)
             : result.ToProblem();
    }
    #endregion

    #region get votes per Question
    [HttpGet("get-votes-per-question/{pollId}")]
    public async Task<IActionResult> GetVotesPerQuestion(int pollId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetQuestionVotesQuery(pollId), cancellationToken);

        return result.IsSuccess
             ? Ok(result.Value)
             : result.ToProblem();
    }
    #endregion


}
