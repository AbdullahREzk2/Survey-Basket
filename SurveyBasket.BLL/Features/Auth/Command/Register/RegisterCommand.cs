namespace SurveyBasket.BLL.Features.Auth.Command.Register;
public record RegisterCommand(RegisterRequestDTO requestDTO) : IRequest<Result>;
