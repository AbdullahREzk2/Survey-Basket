namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles =defaultRoles.Member)]
public class VotesController : ControllerBase
{
    private readonly IQuestionService _questionservice;
    private readonly IVoteService _voteservice;

    public VotesController(IQuestionService questionService,IVoteService voteService)
    {
        _questionservice = questionService;
        _voteservice = voteService;
    }

    #region get availble questions for poll
    [HttpGet("get-availble-questions/{pollId}")]
    public async Task<IActionResult> getAvailble(int pollId, CancellationToken cancellationToken)
    {
       string userId = User.GetUserId()!;

       var result = await _questionservice.GetAvailableQuestionsAsync(pollId, userId, cancellationToken);

         return result.IsSuccess?Ok(result.Value): result.ToProblem();

    }
    #endregion

    #region add vote
    [HttpPost("{pollId}/votes")]
    public async Task<IActionResult> AddVote(int pollId,[FromBody] VoteRequest request,CancellationToken cancellationToken)
    {
        var result = await _voteservice.AddVoteAsync(pollId,User.GetUserId()!,request,cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();

    }
    #endregion

}
