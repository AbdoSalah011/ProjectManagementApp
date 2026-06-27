using ProjectManagement.Infrastructure.Services;

namespace ProjectManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            #region Helpers Variables
            var _jwtSettings =
                configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>();
            #endregion

            // Current User Services
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IJwtService, JwtService>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Register JWT Services & Dependecies
            services.Configure<JwtSettings>(options =>
                configuration.GetSection("JwtSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = _jwtSettings?.Issuer,
                        ValidAudience = _jwtSettings?.Audience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        _jwtSettings?.SecretKey!)),

                        ClockSkew = TimeSpan.Zero
                    };
            });


            return services;
        }
    }
}
