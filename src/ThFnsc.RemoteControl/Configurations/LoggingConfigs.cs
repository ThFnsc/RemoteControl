using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Console;

namespace ThFnsc.RemoteControl.Configurations;

public static class LoggingConfigs
{
    public static WebApplicationBuilder AddCustomLogging(this WebApplicationBuilder builder)
    {
        builder.Services.RemoveAll<ConsoleLoggerProvider>();
        builder.Logging
            .AddSimpleConsole(options =>
            {
                options.TimestampFormat = "\n\n[dd/MM/yy HH:mm:ss 'UTC'] ";
                options.UseUtcTimestamp = true;
                options.IncludeScopes = true;
            });

        builder.Services.AddHttpLogging(options =>
            options.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod | HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.RequestQuery);

        builder.Services.AddProblemDetails(op =>
        {
            op.CustomizeProblemDetails = ctx =>
            {
                if (ctx.ProblemDetails.Status is 404)
                    ctx.ProblemDetails.Detail = "Route not found. Checkout /Swagger";
            };
        });

        return builder;
    }
}
