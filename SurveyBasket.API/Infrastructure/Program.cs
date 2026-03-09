using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SurveyBasket.BLL.Features.Notifications.Command.SendNewPollNotification;
namespace SurveyBasket.API.Infrastructure
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
            );


            builder.Services.AddControllers();

            var app = builder.Build();


            app.MapOpenApi();
            //app.UseSwaggerUI(options=>options.SwaggerEndpoint("/openapi/v1.json","v1"));
            app.MapScalarApiReference();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
                    }
                ],
                DashboardTitle = "Survey Basket Jobs Dashboard",
                //IsReadOnlyFunc = (DashboardContext context) => true

            });

            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            RecurringJob.AddOrUpdate(
                "sendNewNotificationPollAsync",
                () => mediator.Send(new SendNewPollNotificationCommand(null), CancellationToken.None),
                Cron.Daily
            );
            app.UseExceptionHandler();
            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();


            app.MapHealthChecks("health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.Run();


        }
    }
}
