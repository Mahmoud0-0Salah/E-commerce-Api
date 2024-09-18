using Contracts;
using Repository;
using Service.Contracts;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using BookApp.Utility;
using Marvin.Cache.Headers;
using AspNetCoreRateLimit;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookApp
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            /*NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
            {
                return new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>().First();
            }*/

            builder.Services.AddControllers(c =>
            {
                //c.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                c.CacheProfiles.Add("60Age", new CacheProfile
                {
                    VaryByQueryKeys = new[] { "*" },
                    Duration = 60,
                });
            })
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly).AddNewtonsoftJson();


            builder.Services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.deadpool.hateoas+json");
                }
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.AddDbContextPool<RepositoryContext>(opts =>
                            opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddCors(c => {
                c.AddPolicy("Test", cu =>
                {
                    cu.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination");
                });
               });

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<IProductLinks, ProductLinks>();

            builder.Services.AddResponseCaching(o =>
            {
                o.UseCaseSensitivePaths = false;
            });

            builder.Services.AddHttpCacheHeaders(
            (expirationOpt) => 
            {
                expirationOpt.CacheLocation = CacheLocation.Public;
            }, 
            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            });

            builder.Services.AddIdentity<User, IdentityRole>(o=>
            {
                o.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration.GetSection("JWT")["ValidIssuer"],
                ValidAudience = builder.Configuration.GetSection("JWT")["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")["SecretKey"])),
                };
            });

            //////////////// For rate limit //////////////// 
            builder.Services.Configure<IpRateLimitOptions>(opt => {
            opt.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                Endpoint = "*",
                Limit = 20,
                Period = "5m"
                }
                };
            });

            builder.Services.AddSingleton<IRateLimitCounterStore,MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            ////////////////////////////////////////////////////////////////////////////////////////////////


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Details = app.Environment.IsProduction() ? null : contextFeature.Error.ToString()
                        }.ToString());
                    }
                });
            });

            app.UseHttpsRedirection();


            app.UseCors("Test");
        
            app.UseIpRateLimiting();
           
            app.UseResponseCaching();

            app.UseHttpCacheHeaders();


            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}