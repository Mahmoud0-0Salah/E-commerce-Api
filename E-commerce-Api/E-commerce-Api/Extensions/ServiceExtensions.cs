﻿using AspNetCoreRateLimit;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using System.Text;

namespace BookApp.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddeControllers(this IServiceCollection services)
        {
            NewtonsoftJsonInputFormatter GetJsonInputFormatter()
            {
                return new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
                    .Services.BuildServiceProvider()
                    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
                    .OfType<NewtonsoftJsonInputFormatter>().Skip(1).First();
            }


            services.AddControllers(c =>
            {
                c.InputFormatters.Insert(0, GetJsonInputFormatter());
                c.CacheProfiles.Add("60Age", new CacheProfile
                {
                    VaryByQueryKeys = new[] { "*" },
                    Duration = 60,
                });

                c.CacheProfiles.Add("60AgeAuth", new CacheProfile
                {
                    VaryByQueryKeys = new[] { "*" },
                    Duration = 60,
                    Location = ResponseCacheLocation.Client,
                });
            })
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
        }

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.deadpool.hateoas+json");
                }
            });
        }

        public static void ConfigureApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(c => {
                c.AddPolicy("Test", cu =>
                {
                    cu.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination");
                });
            });
        }
        
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<RepositoryContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching(o =>
            {
                o.UseCaseSensitivePaths = false;
            });
        }

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) 
        
        {
            services.AddHttpCacheHeaders((expirationOpt) =>
            {
                expirationOpt.MaxAge = 60;
                expirationOpt.CacheLocation = CacheLocation.Public;
            },
            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
              //  validationOpt.NoCache = true;
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(o =>
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

                    ValidIssuer = configuration.GetSection("JWT")["ValidIssuer"],
                    ValidAudience = configuration.GetSection("JWT")["ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["SecretKey"])),
                };
            });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                     Limit = 100,
                    Period = "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt => { opt.GeneralRules = rateLimitRules; });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }


        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "E-commerce-Api",
                    Version = "v1"
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
            });
        }


    }
}
