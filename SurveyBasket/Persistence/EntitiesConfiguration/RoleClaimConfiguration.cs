namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var permissions = Permissions.GetAllPermissions();
        var adminClaims = new List<IdentityRoleClaim<string>>();

        for(var i = 0; i < permissions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permissions.Type,
                ClaimValue = permissions[i],
                RoleId = defaultRoles.AdminRoleId
            });
        }

        var memberClaims = new List<IdentityRoleClaim<string>>
        {

            new IdentityRoleClaim<string>
            {
                Id = 100,
                ClaimType = Permissions.Type,
                ClaimValue = Permissions.GetPolls,
                RoleId = defaultRoles.MemberRoleId
            },
            new IdentityRoleClaim<string>
            {
                Id = 101,
                ClaimType = Permissions.Type,
                ClaimValue = Permissions.GetQuestions,
                RoleId = defaultRoles.MemberRoleId
            }
        };


        builder.HasData(adminClaims.Concat(memberClaims));

    }
}
