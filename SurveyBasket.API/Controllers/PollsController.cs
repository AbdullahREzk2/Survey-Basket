using SurveyBasket.BLL.Features.Polls.Commands.AddPoll;
using SurveyBasket.BLL.Features.Polls.Commands.DeletePoll;
using SurveyBasket.BLL.Features.Polls.Commands.PublishToggle;
using SurveyBasket.BLL.Features.Polls.Commands.UpdatePoll;
using SurveyBasket.BLL.Features.Polls.Queries.GetAllPolls;
using SurveyBasket.BLL.Features.Polls.Queries.GetAvilablePolls;
using SurveyBasket.BLL.Features.Polls.Queries.GetPollById;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]

public class PollsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    #region getAllPolls
    [HttpGet("getAllPolls")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> GetAllPolls(CancellationToken cancellationToken)
    {
        var pollsResult = await _mediator.Send(new GetAllPollsQuery(),cancellationToken );

        return pollsResult.IsSuccess
            ? Ok(pollsResult.Value)
            : pollsResult.ToProblem();
    }
    #endregion

    #region get Available Polls
    [HttpGet("getAvailablePolls")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> GetAvailablePolls(CancellationToken cancellationToken)
    {
        var pollsResult = await _mediator.Send(new GetAvaliblePollsQuery(),cancellationToken);

        return pollsResult.IsSuccess
            ? Ok(pollsResult.Value)
            : pollsResult.ToProblem();
    }
    #endregion

    #region getPollById
    [HttpGet("getPollById/{pollId}")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> GetPollById(int pollId, CancellationToken cancellationToken)
    {
        var pollResult = await _mediator.Send(new GetPollByIdQuery(pollId), cancellationToken);

        return pollResult.IsSuccess
            ? Ok(pollResult.Value)
            : pollResult.ToProblem();
    }
    #endregion

    #region addPoll
    [HttpPost("addPoll")]
    [HasPermission(Permissions.AddPolls)]
    public async Task<IActionResult> AddPoll([FromBody] PollRequestDTO pollRequest, CancellationToken cancellationToken)
    {
        var createdPoll = await _mediator.Send(new AddPollCommand(pollRequest), cancellationToken);

        return createdPoll.IsSuccess
            ? CreatedAtAction(nameof(GetPollById), new { pollId = createdPoll.Value.PollId }, createdPoll.Value)
            : createdPoll.ToProblem();
    }
    #endregion

    #region updatePoll
    [HttpPut("updatePoll/{pollId}")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> UpdatePoll(int pollId, [FromBody] PollRequestDTO pollRequest, CancellationToken cancellationToken)
    {
        var updatedPoll = await _mediator.Send(new UpdatePollCommand(pollId,pollRequest), cancellationToken);

        return updatedPoll.IsSuccess
            ? Ok(updatedPoll.Value)
            : updatedPoll.ToProblem();
    }
    #endregion

    #region deletePoll
    [HttpDelete("deletePoll/{pollId}")]
    [HasPermission(Permissions.deletePolls)]
    public async Task<IActionResult> DeletePoll(int pollId, CancellationToken cancellationToken)
    {
        var isDeleted = await _mediator.Send(new DeletePollCommand(pollId), cancellationToken);

        return isDeleted.IsSuccess
            ? NoContent()
            : isDeleted.ToProblem();
    }
    #endregion

    #region publishPoll
    [HttpPost("publishPoll/{pollId}")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> PublishPoll(int pollId, CancellationToken cancellationToken)
    {
        var publishedPoll = await _mediator.Send(new PollPublishToggleCommand(pollId), cancellationToken);

        return publishedPoll.IsSuccess
            ? Ok()
            : publishedPoll.ToProblem();
    }
    #endregion

}
