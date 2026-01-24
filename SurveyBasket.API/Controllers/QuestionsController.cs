namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionservice;

    public QuestionsController(IQuestionService questionService)
    {
        _questionservice = questionService;
    }

    #region getAllQuestionsForPoll
    [HttpGet("getAllQuestions / {pollId}")]
    public async Task<IActionResult> getAllQuestionsForPollAsync(int pollId,CancellationToken cancellationToken)
    {
        var questionsResult = await _questionservice.GetAllQuestionsForPollAsync(pollId, cancellationToken);

        return questionsResult.IsSuccess
            ? Ok(questionsResult.Value)
            : questionsResult.ToProblem(statuscode: StatusCodes.Status404NotFound);
    }
    #endregion

    #region getQuestionById
    [HttpGet("getQuestion /{pollId}/{questionId}")]
    public async Task<IActionResult> getQuestionByIdAsync(int pollId,int questionId,CancellationToken cancellationToken)
    {
        var question = await _questionservice.GetQuestionByIdAsync(pollId, questionId, cancellationToken);

        return question.IsSuccess
            ? Ok(question.Value)
            : question.ToProblem(statuscode: StatusCodes.Status404NotFound);
    }
    #endregion

    #region addQuestion
    [HttpPost("addQuestion /{pollId}")]
    public async Task<IActionResult> addQuestionAsync(int pollId,QuestionRequestDTO question ,CancellationToken cancellationToken)
    {
        var addResponse = await _questionservice.AddQuestionAsync(pollId, question, cancellationToken);

        return addResponse.IsSuccess
            ? Ok(addResponse.Value)
            : addResponse.ToProblem(statuscode: StatusCodes.Status400BadRequest);
    }
    #endregion

    #region Update Question
    [HttpPut("{pollId}/questions/{questionId}")]
    public async Task<IActionResult> UpdateQuestionAsync(int pollId,int questionId,[FromBody] QuestionRequestDTO question,CancellationToken cancellationToken)
    {
        var result = await _questionservice
            .UpdateQuestionAsync(pollId, questionId, question, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem(StatusCodes.Status400BadRequest);
    }
    #endregion


}
