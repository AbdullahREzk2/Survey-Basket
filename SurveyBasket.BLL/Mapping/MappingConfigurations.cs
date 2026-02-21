namespace SurveyBasket.BLL.Mapping;
public class MappingConfigurations:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<MappingConfigurations, Poll>().TwoWays();

        config.NewConfig<RegisterRequestDTO, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

          config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
          .Map(dest => dest.Roles, src => src.roles)
          .Map(dest => dest, src => src.user)
            .ConstructUsing(src => new UserResponse(
            src.user.Id,
            src.user.firstName,
            src.user.lastName,
            src.user.Email!,
            src.user.isDisabled,
            src.roles
           ));


    }

}
