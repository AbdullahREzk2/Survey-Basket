namespace SurveyBasket.BLL.Authantication;
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{
}
