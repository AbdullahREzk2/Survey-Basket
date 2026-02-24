namespace SurveyBasket.DAL.IRepository;
public interface IQuestionRepository
{
    IQueryable<Question> GetAllQuestionsForPollAsync(int pollId);
    Task<IReadOnlyList<Question>> GetAvailbaleForPollAsync(int pollId, CancellationToken cancellationToken);
    Task<Question?> GetQuestionByIdAsync(int pollId,int questionId,CancellationToken cancellationToken); 
    Task<Question?> AddQuestionAsync(Question question,CancellationToken cancellationToken);
    Task<bool> UpdateQuestionAsync(Question question, CancellationToken cancellationToken);
    Task<bool> searchQuestion(int pollId,string content,CancellationToken cancellationToken);
    Task<bool> activeToggleQuestion(int pollId, int QuestionId, CancellationToken cancellationToken);
}
