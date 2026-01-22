namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class PollConfiguration : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
       
        builder.HasKey(e => e.PollId);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.HasIndex(e=>e.Title)
            .IsUnique();
    }

}
