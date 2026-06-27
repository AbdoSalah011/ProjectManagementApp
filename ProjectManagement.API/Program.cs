namespace ProjectManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add full Dependencies for all Layers
            builder.Services
                .AddApplicationDependencies()
                .AddPersistenceDependencies(builder.Configuration)
                .AddInfrastructureDependencies(builder.Configuration);

            builder.Services.AddControllers();

            // Api Versioning Configuration
            builder.Services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });

            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();


            // this scope responsiable for run Seeding Data
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>();
                seeder.Seed();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();




            app.Run();
        }
    }
}
