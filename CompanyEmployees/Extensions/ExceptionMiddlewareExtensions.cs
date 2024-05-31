using Contracts.Infrastructure;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace CompanyEmployees.Extensions
{
    /* Extension of UseExceptionHandler Middleware to manage Global Exception in asp.net core api */
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            /*UseExceptionHandler is a built-in middleware*/
            app.UseExceptionHandler(appError =>
            {
                /*Run method represent the terminal middleware*/
                /* the Run method accepts a single parameter of the RequestDelegate type
                 * */
                appError.Run(async context =>
                {

                    
                    using (var scope = app.Services.CreateScope())
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager<ErrorDetails>>();
                        //var logger = app.Services.GetRequiredService<ILoggerManager<ErrorDetails>>();
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            context.Response.StatusCode = contextFeature.Error switch
                            {
                                NotFoundException => StatusCodes.Status404NotFound,
                                BadRequestException => StatusCodes.Status400BadRequest,
                                ValidationAppException => StatusCodes.Status422UnprocessableEntity,
                                _ => StatusCodes.Status500InternalServerError
                            };
                            logger.LogError($"Something went wrong: {contextFeature.Error}");
                            if (contextFeature.Error is ValidationAppException exception)
                            {
                                await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new
                                {
                                    exception.Errors
                                }));
                            }
                            else
                            {
                                await context.Response.WriteAsync(new ErrorDetails()
                                {
                                    StatusCode = context.Response.StatusCode,
                                    Message = contextFeature.Error.Message,
                                }.ToString());
                            }

                        }

                        // your usual code
                    }

                    
                });
            });
        }
    }
}
