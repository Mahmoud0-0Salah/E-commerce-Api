using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BookApp.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (app.Environment.IsProduction())
                        {
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = StatusCodes.Status500InternalServerError,
                                Message = "Internal server error",
                                Details = "We face some problems when try handle your request"
                            }.ToString());
                        }
                        else
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
                                Details = contextFeature.Error.ToString()
                            }.ToString());
                        }
                    }
                });
            });

        }


        

    }
}
