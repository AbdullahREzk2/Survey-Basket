namespace SurveyBasket.BLL.Errors;
public class GlobalExceptionsHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionsHandler> _logger;

    public GlobalExceptionsHandler(ILogger<GlobalExceptionsHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
      _logger.LogError(exception, "Something Went Wrong :{ Message}",exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status/500"
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails,cancellationToken);
        return true;
    }

}
