namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionservice;

    public QuestionsController(IQuestionService questionService)
    {
        _questionservice = questionService;
    }

   #region getAllQuestionsForPoll
    [HttpGet("getAllQuestions / {pollId}")]
    [HasPermission(Permissions.GetQuestions)]
    public async Task<IActionResult> getAllQuestionsForPollAsync(int pollId, [FromQuery] RequestFilters filters,CancellationToken cancellationToken)
    {
        var questionsResult = await _questionservice.GetAllQuestionsForPollAsync(pollId,filters ,cancellationToken);

        return questionsResult.IsSuccess
            ? Ok(questionsResult.Value)
            : questionsResult.ToProblem();
    }
    #endregion

    #region getQuestionById
    [HttpGet("getQuestion /{pollId}/{questionId}")]
    [HasPermission(Permissions.GetQuestions)]

    public async Task<IActionResult> getQuestionByIdAsync(int pollId,int questionId,CancellationToken cancellationToken)
    {
        var question = await _questionservice.GetQuestionByIdAsync(pollId, questionId, cancellationToken);

        return question.IsSuccess
            ? Ok(question.Value)
            : question.ToProblem();
    }
    #endregion

    #region addQuestion
    [HttpPost("addQuestion /{pollId}")]
    [HasPermission(Permissions.AddQuestions)]

    public async Task<IActionResult> addQuestionAsync(int pollId,QuestionRequestDTO question ,CancellationToken cancellationToken)
    {
        var addResponse = await _questionservice.AddQuestionAsync(pollId, question, cancellationToken);

        return addResponse.IsSuccess
            ? Ok(addResponse.Value)
            : addResponse.ToProblem();
    }
    #endregion

    #region Update Question
    [HttpPut("UpdateQuestion/{pollId}/questions/{questionId}")]
    [HasPermission(Permissions.UpdateQuestions)]

    public async Task<IActionResult> UpdateQuestionAsync(int pollId,int questionId,[FromBody] QuestionRequestDTO question,CancellationToken cancellationToken)
    {
        var result = await _questionservice
            .UpdateQuestionAsync(pollId, questionId, question, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    #endregion

    #region Activate Question
    [HttpPut("activateQuestion /{pollId}/{questionId}")]
    [HasPermission(Permissions.UpdateQuestions)]
    public async Task<IActionResult> activateQuestionAsync(int pollId,int questionId,CancellationToken cancellationToken)
    {
        var result = await _questionservice
            .activeToggleQuestion(pollId, questionId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    #endregion


}
