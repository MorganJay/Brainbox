using Domain;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace Api
{
    public static class ServiceExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 400;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature is not null)
                    {
                        //Log.Error($"Something went wrong in {contextFeature.Error}");

                        await context.Response.WriteAsync(new Error
                        {
                            Message = "An unexpected error occured. Try again later",
                            Errors = new List<string> { contextFeature.Error.Message }
                        }.ToString());
                    }
                });
            });
        }
    }
}