using System.Threading.RateLimiting;
using Microsoft.OpenApi;
using SurveyBasket.API.Health;
using SurveyBasket.BLL.Features.Polls.Queries.GetAllPolls;
using SurveyBasket.BLL.Helpers;
using SurveyBasket.BLL.Setting;

namespace SurveyBasket.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
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
                .AddRateLimiting()
                .AddURLServices(configuration)
                .AddOpenApiWithJwt()
                .AddCloudinaryServices(configuration);

            return services;
        }

        // =========================
        // Database
        // =========================
        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )
                ));

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

            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<SendConfirmationEmailHelper>();
            services.AddScoped<SendResetPasswordEmailHelper>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(GetAllPollsQueryHandler).Assembly));

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
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(typeof(PollRequestValidation).Assembly);
            return services;
        }

        // =========================
        // JWT Authentication
        // =========================
        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
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
        private static IServiceCollection AddMailServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<MailSettings>()
                .BindConfiguration(nameof(MailSettings))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<IEmailSender, EmailService>();

            return services;
        }

        // =========================
        // Hangfire
        // =========================
        private static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"))
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
                .AddCheck<MailHealthChecks>(name: "Mail Service");

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

        // =========================
        // OpenAPI + Scalar JWT
        // =========================
        private static IServiceCollection AddOpenApiWithJwt(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Description = "Enter your JWT token"
                    };

                    var securityRequirement = new OpenApiSecurityRequirement();
                    securityRequirement.Add(
                        new OpenApiSecuritySchemeReference("Bearer", document),
                        new List<string>()
                    );

                    document.Security = new List<OpenApiSecurityRequirement> { securityRequirement };

                    return Task.CompletedTask;
                });
            });

            return services;
        }

        // =========================
        // Cloudinary
        // =========================
        private static IServiceCollection AddCloudinaryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CloudinarySettings>()
                .BindConfiguration(CloudinarySettings.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();


            return services;
        }

    }
}