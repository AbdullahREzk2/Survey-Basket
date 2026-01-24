namespace SurveyBasket.DAL.Entities;
public sealed class Question:AuditableEntity
{
    public int questionId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool isActive { get; set; } = true;

    public int PollId { get; set; }
    public Poll poll { get; set; } = default!;
    public ICollection<Answer> answers { get; set; } = [];
}
