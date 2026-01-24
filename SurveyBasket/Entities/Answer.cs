namespace SurveyBasket.DAL.Entities;
public sealed class Answer:AuditableEntity
{
    public int answerId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool isActive { get; set; } = true;
    public int questionId { get; set; }
    public Question question { get; set; } = default!;
}
