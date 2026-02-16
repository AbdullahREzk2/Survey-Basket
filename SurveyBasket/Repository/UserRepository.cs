namespace SurveyBasket.DAL.Repository;
public class UserRepository:IUserRepository
{
    private readonly ApplicationDBContext _dbcontext;

    public UserRepository(ApplicationDBContext dBContext)
    {
        _dbcontext = dBContext;
    }

    public async Task<IList<string>> GetUserPermissions(IList<string> userRoles,CancellationToken cancellationToken)
    {
        var userPermissions = await (from r in _dbcontext.Roles
                                     join p in _dbcontext.RoleClaims
                                     on r.Id equals p.RoleId
                                     where userRoles.Contains(r.Name!)
                                     select p.ClaimValue)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);

        return userPermissions!;
    }


}
