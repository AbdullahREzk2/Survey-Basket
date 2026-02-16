namespace SurveyBasket.DAL.Entities;
public class ApplicationRole:IdentityRole
{
    public bool isDeafult { get; set; }
    public bool isDeleted { get; set; }
}
