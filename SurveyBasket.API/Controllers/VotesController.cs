using SurveyBasket.BLL.Features.Questions.Query.GetAvailableQuestion;
using SurveyBasket.BLL.Features.Votes.Command;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = defaultRoles.Member.Name)]
public class VotesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    #region get availble questions for poll
    [HttpGet("get-availble-questions/{pollId}")]
    public async Task<IActionResult> getAvailble(int pollId, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(new GetAvailableQuestionQuery(pollId,User.GetUserId()!));

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }
    #endregion

    #region add vote
    [HttpPost("{pollId}/votes")]
    public async Task<IActionResult> AddVote(int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddVoteCommand(pollId, User.GetUserId()!, request));

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();

    }
    #endregion

}
