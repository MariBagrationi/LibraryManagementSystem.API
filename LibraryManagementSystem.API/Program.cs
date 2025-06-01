using Asp.Versioning;
using FluentValidation;
using LibraryManagementSystem.API.Infrastructure.Auth.JWT;
using LibraryManagementSystem.API.Infrastructure.Extensions;
using LibraryManagementSystem.API.Infrastructure.Mappings;
using LibraryManagementSystem.API.Infrastructure.Middlewares;
using LibraryManagementSystem.Persistance.Connections;
using LibraryManagementSystem.Persistance.Context;
using LibraryManagementSystem.Persistance.Seed;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

namespace LibraryManagementSystem.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();
              

            builder.Host.UseSerilog();

            #region versioning

            //builder.Services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //    //options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.ApiVersionReader = new UrlSegmentApiVersionReader();

            //    // Reject unknown API versions
            //    //options.ReportApiVersions = true;
            //    //options.UnsupportedApiVersionStatusCode = StatusCodes.Status400BadRequest;
            //}).AddApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "'v'V";
            //    options.SubstituteApiVersionInUrl = true;

            //});

            #endregion

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerConfiguration();


            builder.Services.AddLogging();

            builder.Services.AddServices();
            builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection(nameof(JWTConfig)));

            builder.Services.AddTokenAuthentication(builder.Configuration["JWTConfig:Secret"]!);

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.RegisterMaps();

            builder.Services.AddDbContext<LibraryManagementContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection))));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    await LibraryManagementSeed.InitializeAsync(services);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error seeding database: {ex.Message}");
                }
            }

            app.UseMiddleware<RequestResponseLogger>();
            app.UseMiddleware<ExceptionHandler>();

         
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
