namespace SurveyBasket.DAL.Entities;
public class AuditableEntity
{
    public string createdById { get; set; } = string.Empty;
    public DateTime createdOn { get; set; } = DateTime.UtcNow;

    public string? updatedById { get; set; }
    public DateTime? updatedOn { get; set; }

    public ApplicationUser createdBy { get; set; } = default!;
    public ApplicationUser? updatedBy { get; set; }

}
