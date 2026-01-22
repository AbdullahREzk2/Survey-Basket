using SurveyBasket.BLL.Contracts.Polls;

namespace SurveyBasket.BLL.IService;
public interface IPollService
{
    Task<IEnumerable<PollResponseDTO>> getAllPollsAsync(CancellationToken cancellationToken);
    Task<PollResponseDTO> getPollByIdAsync(int pollId,CancellationToken cancellationToken);
    Task<PollResponseDTO?> AddPollAsync(PollRequestDTO poll,CancellationToken cancellationToken);
    Task<PollResponseDTO?> UpdatePollAsync(int pollId, PollRequestDTO poll,CancellationToken cancellationToken);
    Task<bool> DeletePollAsync(int pollId, CancellationToken cancellationToken);
}
