using HangfireBasicAuthenticationFilter;

namespace SurveyBasket.API.Infrastructure
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Host.UseSerilog((context,configuration)=>
            configuration.ReadFrom.Configuration(context.Configuration)
            );


            builder.Services.AddControllers()
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<PollRequestValidation>();
            });

            builder.Services.AddOpenApi();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                //app.UseSwaggerUI(options=>options.SwaggerEndpoint("/openapi/v1.json","v1"));
                app.MapScalarApiReference();
            }
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseHangfireDashboard("/jobs",new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
                    }
                ],
                DashboardTitle= "Survey Basket Jobs Dashboard"

            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseExceptionHandler();

            app.Run();


        }
    }
}
