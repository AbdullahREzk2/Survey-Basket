namespace SurveyBasket.BLL.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        _logger.LogInformation("⚡ Starting {RequestName} at {DateTime}", requestName, DateTime.UtcNow);

        try
        {
            var response = await next();
            _logger.LogInformation("✅ Completed {RequestName} at {DateTime}", requestName, DateTime.UtcNow);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error in {RequestName}", requestName);
            throw;
        }
    }
}