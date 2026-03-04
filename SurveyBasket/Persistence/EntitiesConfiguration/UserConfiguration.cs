namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .OwnsMany(u => u.refreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(u => u.firstName)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.lastName)
            .HasMaxLength(50)
            .IsRequired();

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        //default Data 
        builder.HasData(new ApplicationUser
        {

            Id = defaultUser.Admin.Id,
            firstName = "Admin",
            lastName = "Admin",
            UserName = defaultUser.Admin.Email,
            NormalizedUserName = defaultUser.Admin.Email.ToUpper(),
            Email = defaultUser.Admin.Email,
            NormalizedEmail = defaultUser.Admin.Email.ToUpper(),
            SecurityStamp = defaultUser.Admin.SecurityStamp,
            ConcurrencyStamp = defaultUser.Admin.ConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = defaultUser.Admin.PasswordHash
        });

    }
}
