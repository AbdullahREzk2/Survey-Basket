namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasIndex(x => new {x.questionId,x.Content}).IsUnique();

        builder.Property(x=>x.Content)
               .IsRequired()
               .HasMaxLength(1000);
    }

}
