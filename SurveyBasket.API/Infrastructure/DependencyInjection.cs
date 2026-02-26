using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.API.Health;

namespace SurveyBasket.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services
                .AddDatabase(configuration)
                .AddIdentityServices()
                .AddRepositories()
                .AddBusinessServices()
                .AddMapster()
                .AddFluentValidation()
                .AddJwtAuthentication(configuration)
                .AddExceptionHandling()
                .AddMailServices(configuration)
                .AddHangfireServices(configuration)
                .AddhealthChecks()
                .AddRateLimiting();


            return services;
        }

        // =========================
        // Database
        // =========================
        private static IServiceCollection AddDatabase(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        // =========================
        // Identity
        // =========================
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        // =========================
        // Repositories (DAL)
        // =========================
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPollRepository, PollRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }

        // =========================
        // Business Services (BLL)
        // =========================
        private static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }

        // =========================
        // Mapster
        // =========================
        private static IServiceCollection AddMapster(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(typeof(MappingConfigurations).Assembly);

            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }

        // =========================
        // Fluent Validation
        // =========================
        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(PollRequestValidation).Assembly);
            return services;
        }

        // =========================
        // JWT Authentication
        // =========================
        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
                configuration.GetSection(JwtOptions.sectionName));

            var jwtSetting = configuration
                .GetSection(JwtOptions.sectionName)
                .Get<JwtOptions>();

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.sectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSetting!.Issuer,
                    ValidAudience = jwtSetting!.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSetting.key!))
                };
            });

            return services;
        }

        // =========================
        // Exception Handling
        // =========================
        private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionsHandler>();
            services.AddProblemDetails();

            return services;
        }

        // =========================
        // Mail Settings
        // =========================
        private static IServiceCollection AddMailServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<MailSettings>(
                configuration.GetSection(nameof(MailSettings)));

            services.AddScoped<IEmailSender, EmailService>();

            return services;
        }

        // =========================
        // Hangfire
        // =========================
        private static IServiceCollection AddHangfireServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(
                        configuration.GetConnectionString("HangfireConnection")
                    );
                })
            );

            services.AddHangfireServer();

            return services;
        }

        // =========================
        // APPURL settings
        // =========================
        private static IServiceCollection AddURLServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppURLSetting>(
                configuration.GetSection("AppURL"));

            return services;
        }

        // =========================
        // Health checks
        // =========================
        private static IServiceCollection AddhealthChecks(this IServiceCollection services)
        {

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDBContext>(name: "database")
                .AddHangfire(options => { options.MinimumAvailableServers = 1; })
                .AddCheck<MailHealthChecks>(name:"Mail Service");

            return services;
        }

        // =========================
        // Rate Limiting
        // =========================
        private static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("ipLimit", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(10)
                        }
                    ));
            });

            return services;
        }


    }
}
