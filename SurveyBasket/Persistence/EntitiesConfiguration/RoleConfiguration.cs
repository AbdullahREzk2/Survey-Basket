namespace SurveyBasket.DAL.Persistence.EntitiesConfiguration;
public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new ApplicationRole
            {
                Id = defaultRoles.Admin.RoleId,
                Name = defaultRoles.Admin.Name,
                NormalizedName = defaultRoles.Admin.Name.ToUpper(),
                ConcurrencyStamp = defaultRoles.Admin.RoleConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = defaultRoles.Member.RoleId,
                Name = defaultRoles.Member.Name,
                NormalizedName = defaultRoles.Member.Name.ToUpper(),
                ConcurrencyStamp = defaultRoles.Member.RoleConcurrencyStamp,
                isDeafult = true
            }


        ]);
    }

}
