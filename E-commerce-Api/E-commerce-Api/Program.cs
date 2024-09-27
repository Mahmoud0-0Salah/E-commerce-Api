using Contracts;
using Repository;
using Service.Contracts;
using Service;
using BookApp.Utility;
using AspNetCoreRateLimit;
using BookApp.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

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

            //builder.Services.ConfigureResponseCaching();

            builder.Services.ConfigureHttpCacheHeaders();

            builder.Services.ConfigureIdentity();

            builder.Services.ConfigureJWT(builder.Configuration);
           
            builder.Services.ConfigureRateLimitingOptions();
      
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.AddScoped<IProductLinks, ProductLinks>();
      
            builder.Services.AddScoped<IUserLinks, UserLinks>();

            builder.Services.AddScoped<ICatgoryLinks, CateogriesLinks>();

            builder.Services.ConfigureSwagger();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "E-commerce-Api v1");
                s.RoutePrefix = string.Empty;
            });

            app.ConfigureExceptionHandler();

            app.UseCors("Test");
        
            app.UseIpRateLimiting();

          //  app.UseResponseCaching();

         //   app.UseHttpCacheHeaders();
            
            app.UseHttpsRedirection();
         
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}