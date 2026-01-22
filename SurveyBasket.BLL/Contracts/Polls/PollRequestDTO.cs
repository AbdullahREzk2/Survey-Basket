namespace SurveyBasket.BLL.Contracts.Polls;
public class PollRequestDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly startDate { get; set; }
    public DateOnly endDate { get; set; }

}
