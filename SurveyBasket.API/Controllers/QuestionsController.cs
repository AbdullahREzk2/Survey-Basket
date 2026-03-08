using SurveyBasket.BLL.Features.Questions.Command.ActiveToggleQuestion;
using SurveyBasket.BLL.Features.Questions.Command.AddQuestion;
using SurveyBasket.BLL.Features.Questions.Command.UpdateQuestion;
using SurveyBasket.BLL.Features.Questions.Query.GetAllQuestionsForPoll;
using SurveyBasket.BLL.Features.Questions.Query.GetQuestionById;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class QuestionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    #region getAllQuestionsForPoll
    [HttpGet("getAllQuestions / {pollId}")]
    [HasPermission(Permissions.GetQuestions)]
    public async Task<IActionResult> getAllQuestionsForPollAsync(int pollId, [FromQuery] RequestFilters filters)
    {
        var questionsResult = await _mediator.Send(new GetAllQuestionsForPollQuery(pollId, filters));

        return questionsResult.IsSuccess
            ? Ok(questionsResult.Value)
            : questionsResult.ToProblem();
    }
    #endregion

    #region getQuestionById
    [HttpGet("getQuestion /{pollId}/{questionId}")]
    [HasPermission(Permissions.GetQuestions)]

    public async Task<IActionResult> getQuestionByIdAsync(int pollId, int questionId)
    {
        var question = await _mediator.Send(new GetQuestionByIdQuery(pollId, questionId));

        return question.IsSuccess
            ? Ok(question.Value)
            : question.ToProblem();
    }
    #endregion

    #region addQuestion
    [HttpPost("addQuestion /{pollId}")]
    [HasPermission(Permissions.AddQuestions)]

    public async Task<IActionResult> addQuestionAsync(int pollId, QuestionRequestDTO question, CancellationToken cancellationToken)
    {
        var addResponse = await _mediator.Send(new AddQuestionCommand(pollId, question));

        return addResponse.IsSuccess
            ? Ok(addResponse.Value)
            : addResponse.ToProblem();
    }
    #endregion

    #region Update Question
    [HttpPut("UpdateQuestion/{pollId}/questions/{questionId}")]
    [HasPermission(Permissions.UpdateQuestions)]

    public async Task<IActionResult> UpdateQuestionAsync(int pollId, int questionId, [FromBody] QuestionRequestDTO question, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateQuestionCommand(pollId, questionId, question));

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    #endregion

    #region Activate Question
    [HttpPut("activateQuestion /{pollId}/{questionId}")]
    [HasPermission(Permissions.UpdateQuestions)]
    public async Task<IActionResult> activateQuestionAsync(int pollId, int questionId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ActiveToggleQuestionCommand(pollId, questionId));

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    #endregion


}
