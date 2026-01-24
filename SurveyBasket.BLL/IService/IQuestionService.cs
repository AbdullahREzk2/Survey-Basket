namespace SurveyBasket.BLL.IService;
public interface IQuestionService
{
    Task<Result<IReadOnlyList<QuestionResponseDTO>>> GetAllQuestionsForPollAsync(int pollId, CancellationToken cancellationToken);
    Task<Result<QuestionResponseDTO>> GetQuestionByIdAsync(int pollId, int questionId, CancellationToken cancellationToken);
    Task<Result<QuestionResponseDTO>> AddQuestionAsync(int pollId,QuestionRequestDTO question, CancellationToken cancellationToken);
    Task<Result> UpdateQuestionAsync(int pollId,int questionId,QuestionRequestDTO question, CancellationToken cancellationToken);
}
