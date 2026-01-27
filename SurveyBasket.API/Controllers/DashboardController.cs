namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardservice;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardservice = dashboardService;
    }

    #region Dashboard-statics
    [HttpGet("get-Dashboard-statics/{pollId}")]
    public async Task<IActionResult> DashboardStatics(int pollId,CancellationToken cancellationToken)
    {
        var result = await _dashboardservice.GetPollVotesAsync(pollId, cancellationToken);
        return result.IsSuccess
             ? Ok(result.Value)
             : result.ToProblem();
    }
    #endregion

    #region getVotesPerDay
    [HttpGet("get-votes-per-day/{pollId}")]
    public async Task<IActionResult> GetVotesPerDay(int pollId, CancellationToken cancellationToken)
    {
        var result = await _dashboardservice.getVotesPerDay(pollId, cancellationToken);

        return result.IsSuccess
             ? Ok(result.Value)
             : result.ToProblem();
    }
    #endregion

    #region get votes per Question
    [HttpGet("get-votes-per-question/{pollId}")]
    public async Task<IActionResult> GetVotesPerQuestion(int pollId, CancellationToken cancellationToken)
    {
        var result = await _dashboardservice.GetQuestionVotesAsync(pollId, cancellationToken);

        return result.IsSuccess
             ? Ok(result.Value)
             : result.ToProblem();
    }
    #endregion


}
