namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .OwnsMany(u=>u.refreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(u=> u.firstName)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.lastName)
            .HasMaxLength(50)
            .IsRequired();

    }
}
