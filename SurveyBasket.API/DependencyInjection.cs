namespace SurveyBasket.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {

            // =========================
            // Database
            // =========================
            #region
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseNpgsql(connectionString));
            #endregion

            //==========================
            // Identity
            //==========================
            #region
            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
            #endregion

            // =========================
            // Repositories (DAL)
            // =========================
            #region
            services.AddScoped<IPollRepository, PollRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            #endregion

            // =========================
            // Business Services (BLL)
            // =========================
            #region
            services.AddHttpContextAccessor();
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IQuestionService, QuestionService>();
            #endregion

            // =========================
            // Mapester
            // =========================
            #region
            var MappingConfig = TypeAdapterConfig.GlobalSettings;
            MappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(MappingConfig));
            #endregion

            // =========================
            // Fluent Validation
            // =========================
            #region
            services.AddValidatorsFromAssembly(typeof(PollRequestValidation).Assembly);
            #endregion


            // =========================
            // Authentication
            // =========================
            #region
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.sectionName));

            var jwtSetting=configuration.GetSection(JwtOptions.sectionName).Get<JwtOptions>();

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.sectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddAuthentication(options=>
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.key!)),
                    };
                });

            #endregion
            // =========================
            // Exception Handling
            // =========================
            #region
            services.AddExceptionHandler<GlobalExceptionsHandler>();
            services.AddProblemDetails();
            #endregion

            

            return services;
        
        }
    }
}
