namespace SurveyBasket.DAL.IRepository;
public interface IUserRepository
{
    Task<IList<string>> GetUserPermissions(IList<string> userRoles,CancellationToken cancellationToken);
}
