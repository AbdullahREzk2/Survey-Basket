namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(new IdentityUserRole<string>
        {
            UserId = defaultUser.AdminId,
            RoleId = defaultRoles.AdminRoleId,
        });
    }

}
