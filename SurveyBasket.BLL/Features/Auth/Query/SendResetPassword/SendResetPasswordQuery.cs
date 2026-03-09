namespace SurveyBasket.BLL.Features.Auth.Query.SendResetPassword;
public record SendResetPasswordQuery(ForgetPasswordRequest passRequest) : IRequest<Result>;
