namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
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

        if (result.IsSuccess)
            return Ok(result.Value);

        return result.Error.Equals(VoteErrors.UserAlreadyVoted)
            ? result.ToProblem(StatusCodes.Status409Conflict)
            : result.ToProblem(StatusCodes.Status404NotFound);
    }
    #endregion

    #region add vote
    [HttpPost("{pollId}/votes")]
    public async Task<IActionResult> AddVote(int pollId,[FromBody] VoteRequest request,CancellationToken cancellationToken)
    {
        var result = await _voteservice.AddVoteAsync(pollId,User.GetUserId()!,request,cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return result.Error switch
        {
            var e when e == VoteErrors.UserAlreadyVoted =>
                result.ToProblem(StatusCodes.Status409Conflict),

            var e when e == PollErrors.PollNotFound =>
                result.ToProblem(StatusCodes.Status404NotFound),

            _ =>
                result.ToProblem(StatusCodes.Status400BadRequest)
        };
    }
    #endregion

}
