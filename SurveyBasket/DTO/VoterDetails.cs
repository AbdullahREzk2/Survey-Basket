namespace SurveyBasket.DAL.DTO;
public class VoterDetails
{
    public int VoteId { get; set; }
    public string VoterName { get; set; } = default!;
    public DateTime VoteDate { get; set; }
}
