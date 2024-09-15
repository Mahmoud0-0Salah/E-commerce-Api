using Contracts;
using Repository;
using Service.Contracts;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Shared.ActionFilters;
using Presentation.ActionFilters;
using BookApp.Utility;

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

            /*builder.Services.AddControllers(options =>
            {
                foreach (var formatter in options.InputFormatters)
                {
                    Console.WriteLine($"Formatter Type: {formatter.GetType().FullName}");
                }

            });*/

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

            builder.Services.AddScoped<ValidationFilterAttribute>();

            builder.Services.AddScoped<ValidateMediaTypeAttribute>();

            builder.Services.AddScoped<IProductLinks, ProductLinks>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}