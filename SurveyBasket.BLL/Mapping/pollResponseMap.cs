namespace SurveyBasket.BLL.Mapping;
public class pollResponseMap:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<pollResponseMap, Poll>().TwoWays();
    }

}
