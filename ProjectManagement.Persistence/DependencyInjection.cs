namespace ProjectManagement.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Application DbContext
            services.AddDbContextPool<ApplicationDbContext>(
                (sp, options) =>
                {
                    var interceptors = sp.GetServices<IInterceptor>();

                    options.UseLazyLoadingProxies()
                           .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                    options.AddInterceptors(interceptors);
                });

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                // Configure password constraints or properties if needed
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // DataSeeder Services
            services.AddScoped<IDataSeeder, DataSeeder>();

            // UnitOfWork Dependency
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Repositories Services
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
