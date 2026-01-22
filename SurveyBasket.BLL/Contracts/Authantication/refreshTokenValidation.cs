namespace SurveyBasket.BLL.Contracts.Authantication;
public class refreshTokenValidation:AbstractValidator<refreshTokenRequest>
{
    public refreshTokenValidation()
    {
        RuleFor(x=>x.Token)
            .NotEmpty()
            .WithMessage("Token is required");
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh Token is required");
    }

}
