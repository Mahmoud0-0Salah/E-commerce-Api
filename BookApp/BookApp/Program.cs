using Contracts;
using Repository;
using Service.Contracts;
using Service;
using BookApp.Utility;
using AspNetCoreRateLimit;
using BookApp.Extensions;

namespace BookApp
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.

            builder.Services.AddeControllers();

            builder.Services.AddCustomMediaTypes();

            builder.Services.ConfigureApiBehaviorOptions();

            builder.Services.ConfigureSqlContext(builder.Configuration);

            builder.Services.ConfigureCors();

            builder.Services.ConfigureResponseCaching();

            builder.Services.ConfigureHttpCacheHeaders();

            builder.Services.ConfigureIdentity();

            builder.Services.ConfigureJWT(builder.Configuration);

            builder.Services.ConfigureRateLimitingOptions();
      
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.AddScoped<IProductLinks, ProductLinks>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.ConfigureExceptionHandler();

            app.UseCors("Test");
        
            app.UseIpRateLimiting();

            app.UseResponseCaching();
            
            app.UseHttpsRedirection();
         
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}