namespace SurveyBasket.DAL.Entities;
public sealed class Vote
{
    public int voteId { get; set; }
    public int PollId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime submittedOn { get; set; }=DateTime.UtcNow;


    public Poll Poll { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
    public ICollection<VoteAnswer> voteAnswers { get; set; } = [];
}
