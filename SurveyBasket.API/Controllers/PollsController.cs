namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]

public class PollsController : ControllerBase
{
    private readonly IPollService _pollservice;

    public PollsController(IPollService pollService)
    {
        _pollservice = pollService;
    }

    #region getAllPolls
    [HttpGet("getAllPolls")]
    public async Task<IActionResult> GetAllPolls(CancellationToken cancellationToken)
    {
        var pollsResult = await _pollservice.getAllPollsAsync(cancellationToken);

        return pollsResult.IsSuccess
            ? Ok(pollsResult.Value) 
            : pollsResult.ToProblem(statuscode:StatusCodes.Status404NotFound);
    }
    #endregion

    #region getPollById
    [HttpGet("getPollById/{pollId}")]
    public async Task<IActionResult> GetPollById(int pollId, CancellationToken cancellationToken)
    {
        var pollResult = await _pollservice.getPollByIdAsync(pollId, cancellationToken);
      
        return pollResult.IsSuccess 
            ? Ok(pollResult.Value) 
            : pollResult.ToProblem(statuscode:StatusCodes.Status404NotFound);
    }
    #endregion

    #region addPoll
    [HttpPost("addPoll")]
    public async Task<IActionResult> AddPoll([FromBody] PollRequestDTO pollRequest, CancellationToken cancellationToken)
    {
        var createdPoll = await _pollservice.AddPollAsync(pollRequest, cancellationToken);

        return createdPoll.IsSuccess 
            ? CreatedAtAction(nameof(GetPollById), new { pollId = createdPoll.Value.PollId }, createdPoll.Value)
            : createdPoll.ToProblem(statuscode: StatusCodes.Status400BadRequest);
    }
    #endregion

    #region updatePoll
    [HttpPut("updatePoll/{pollId}")]
    public async Task<IActionResult> UpdatePoll(int pollId, [FromBody] PollRequestDTO pollRequest, CancellationToken cancellationToken)
    {
        var updatedPoll = await _pollservice.UpdatePollAsync(pollId, pollRequest, cancellationToken);
            
        return updatedPoll.IsSuccess 
            ? Ok(updatedPoll.Value) 
            : updatedPoll.ToProblem(statuscode:StatusCodes.Status404NotFound);
    }
    #endregion

    #region deletePoll
    [HttpDelete("deletePoll/{pollId}")]
    public async Task<IActionResult> DeletePoll(int pollId, CancellationToken cancellationToken)
    {
        var isDeleted = await _pollservice.DeletePollAsync(pollId, cancellationToken);
       
        return isDeleted.IsSuccess 
            ? NoContent() 
            : isDeleted.ToProblem(statuscode:StatusCodes.Status404NotFound);
    }
    #endregion


}
