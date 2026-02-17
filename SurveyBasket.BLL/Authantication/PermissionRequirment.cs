namespace SurveyBasket.BLL.Authantication;
public class PermissionRequirment(string permission):IAuthorizationRequirement
{
    public string Permission { get; } = permission;

}
