namespace SurveyBasket.DAL.Repository;
public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDBContext _context;

    public QuestionRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Question>> GetAllQuestionsForPollAsync(int pollId,CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Where(q => q.PollId == pollId)
            .Include(q=>q.answers.Where(a=>a.isActive))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<Question>> GetAvailbaleForPollAsync(int pollId, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Where(q => q.PollId == pollId && q.isActive)
            .Include(q => q.answers.Where(a => a.isActive))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<Question?> GetQuestionByIdAsync(int pollId,int questionId,CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Where(q=> q.PollId == pollId && q.questionId == questionId)
            .Include(q=>q.answers.Where(a=>a.isActive))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Question?> AddQuestionAsync(Question question,CancellationToken cancellationToken)
    {
        _context.Questions.Add(question);

        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Questions
            .Include(q=>q.answers)
            .FirstOrDefaultAsync(q=>q.questionId==question.questionId,cancellationToken);

    }
    public async Task<bool> UpdateQuestionAsync(Question question,CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<bool> searchQuestion(int pollId, string content, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .AnyAsync(q => q.PollId == pollId && q.Content == content, cancellationToken);
    }
    public async Task<bool> activeToggleQuestion(int pollId, int QuestionId, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Where(q=> q.PollId == pollId && q.questionId == QuestionId)
            .FirstOrDefaultAsync(cancellationToken);

         if (question == null)
            return false;

        question.isActive = !question.isActive;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }


}
