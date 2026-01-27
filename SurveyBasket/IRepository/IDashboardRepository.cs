namespace SurveyBasket.DAL.IRepository;
public interface IDashboardRepository
{
    // get poll title 
    Task<string?> GetPollTitleAsync(int pollId, CancellationToken cancellationToken = default);
    // get vetorName , Date
    Task<IEnumerable<VoterDetails>> GetVoterDetailsAsync(int pollId,CancellationToken cancellationToken = default);
    // get Question and it's answer 
    Task<IEnumerable<QuestionAnswerDTO>> GetQuestionAnswersAsync(int pollId, CancellationToken cancellationToken = default);
    // get votes per day
    Task <IEnumerable<VotesPerDayDTO>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default);
    // get votes per question
    Task<IEnumerable<QuestionVoteDTO>> getVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default); 
    
}
