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

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        //default Data 
        builder.HasData(new ApplicationUser
        {

            Id= defaultUser.AdminId,
            firstName= "Admin",
            lastName= "Admin",
            UserName= defaultUser.AdminEmail,
            NormalizedUserName= defaultUser.AdminEmail.ToUpper(),
            Email= defaultUser.AdminEmail,
            NormalizedEmail= defaultUser.AdminEmail.ToUpper(),
            SecurityStamp= defaultUser.AdminSecurityStamp,
            ConcurrencyStamp= defaultUser.AdminConcurrencyStamp,
            EmailConfirmed= true,
            PasswordHash = defaultUser.AdminPasswordHash
        });

    }
}
