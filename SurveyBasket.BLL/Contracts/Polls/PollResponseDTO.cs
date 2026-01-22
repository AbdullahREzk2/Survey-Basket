namespace SurveyBasket.BLL.Contracts.Polls;
public class PollResponseDTO
{
    public int PollId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool isPublished { get; set; }
    public DateOnly startDate { get; set; }
    public DateOnly endDate { get; set; }

}
