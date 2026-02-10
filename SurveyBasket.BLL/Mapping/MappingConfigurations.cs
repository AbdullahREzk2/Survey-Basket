namespace SurveyBasket.BLL.Mapping;
public class MappingConfigurations:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<MappingConfigurations, Poll>().TwoWays();

        config.NewConfig<RegisterRequestDTO, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);
    }

}
