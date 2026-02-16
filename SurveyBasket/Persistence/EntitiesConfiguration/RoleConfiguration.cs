namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new ApplicationRole
            {
                Id = defaultRoles.AdminRoleId,
                Name = defaultRoles.Admin,
                NormalizedName = defaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = defaultRoles.AdminRoleConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = defaultRoles.MemberRoleId,
                Name = defaultRoles.Member,
                NormalizedName = defaultRoles.Member.ToUpper(),
                ConcurrencyStamp = defaultRoles.MemberRoleConcurrencyStamp,
                isDeafult = true
            }


        ]);
    }

}
