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
        var polls = await _pollservice.getAllPollsAsync(cancellationToken);
        if(polls == null || !polls.Any())
        {
            return NotFound("No polls found.");
        }
        return Ok(polls);
    }
    #endregion

    #region getPollById
    [HttpGet("getPollById/{pollId}")]
    public async Task<IActionResult> GetPollById(int pollId, CancellationToken cancellationToken)
    {
        var poll = await _pollservice.getPollByIdAsync(pollId, cancellationToken);
        if(poll == null)
        {
            return NotFound($"Poll with ID {pollId} not found.");
        }
        return Ok(poll);
    }
    #endregion

    #region addPoll
    [HttpPost("addPoll")]
    public async Task<IActionResult> AddPoll([FromBody] PollRequestDTO pollRequest, CancellationToken cancellationToken)
    {
        
        var createdPoll = await _pollservice.AddPollAsync(pollRequest, cancellationToken);

        if(createdPoll == null)
            return BadRequest("Failed to create poll.");

        return CreatedAtAction(nameof(GetPollById), new { pollId = createdPoll.PollId }, createdPoll);
    }
    #endregion

    #region updatePoll
    [HttpPut("updatePoll/{pollId}")]
    public async Task<IActionResult> UpdatePoll(int pollId, [FromBody] PollRequestDTO pollRequest, CancellationToken cancellationToken)
    {
        var updatedPoll = await _pollservice.UpdatePollAsync(pollId, pollRequest, cancellationToken);
        if(updatedPoll == null)
        {
            return NotFound($"Poll with ID {pollId} not found.");
        }
        return Ok(updatedPoll);
    }
    #endregion

    #region deletePoll
    [HttpDelete("deletePoll/{pollId}")]
    public async Task<IActionResult> DeletePoll(int pollId, CancellationToken cancellationToken)
    {
        var isDeleted = await _pollservice.DeletePollAsync(pollId, cancellationToken);
        if(!isDeleted)
        {
            return NotFound($"Poll with ID {pollId} not found.");
        }
        return NoContent();
    }
    #endregion


}
