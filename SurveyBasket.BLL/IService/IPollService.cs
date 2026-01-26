namespace SurveyBasket.BLL.IService;
public interface IPollService
{
    Task<Result<IEnumerable<PollResponseDTO>>> getAllPollsAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<PollResponseDTO>>> getAvailblePollsAsync(CancellationToken cancellationToken);
    Task<Result<PollResponseDTO>> getPollByIdAsync(int pollId,CancellationToken cancellationToken);
    Task<Result<PollResponseDTO>> AddPollAsync(PollRequestDTO poll,CancellationToken cancellationToken);
    Task<Result<PollResponseDTO>> UpdatePollAsync(int pollId, PollRequestDTO poll,CancellationToken cancellationToken);
    Task<Result> DeletePollAsync(int pollId, CancellationToken cancellationToken);
    Task<Result> publishToggle(int pollId, CancellationToken cancellationToken);    
}
