namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VotesController : ControllerBase
{
    private readonly IQuestionService _questionservice;

    public VotesController(IQuestionService questionService)
    {
        _questionservice = questionService;
    }

    #region get availble questions for poll
    [HttpGet("get-availble-questions/{pollId}")]
    public async Task<IActionResult> getAvailble(int pollId, CancellationToken cancellationToken)
    {
       string userId = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)!;
       var result = await _questionservice.GetAvailableQuestionsAsync(pollId, userId, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return result.Error.Equals(VoteErrors.UserAlreadyVoted)
            ? result.ToProblem(StatusCodes.Status409Conflict)
            : result.ToProblem(StatusCodes.Status404NotFound);
    }
    #endregion

}
