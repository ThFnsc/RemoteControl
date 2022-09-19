using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging.Console;
using System.Reflection;
using ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;
using ThFnsc.RemoteControl.AuthHandlers.QueryString;

namespace ThFnsc.RemoteControl;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                ? AppContext.BaseDirectory
                : default
        });

        using var settingsStream = typeof(Program).Assembly.GetManifestResourceStream(typeof(Program), "appsettings.json");
        builder.Configuration
            .AddJsonStream(settingsStream)
            .AddJsonFile("preferences.json", optional: false, reloadOnChange: true);

        builder.Services.RemoveAll<ConsoleLoggerProvider>();
        builder.Logging
            .AddSimpleConsole(options =>
            {
                options.TimestampFormat = "\n\n[dd/MM/yy HH:mm:ss 'UTC'] ";
                options.UseUtcTimestamp = true;
                options.IncludeScopes = true;
            });

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.Configure<QueryStringAuthenticationOptions>(QueryStringAuthenticationDefaults.AuthenticationScheme, builder.Configuration.GetSection(nameof(QueryStringAuthenticationOptions)));
        builder.Services.Configure<AntiBruteforceOptions>(builder.Configuration.GetSection(nameof(AntiBruteforceOptions)));

        builder.Services.AddAuthentication(QueryStringAuthenticationDefaults.AuthenticationScheme)
            .UseAntiBruteforce(
                builderAction: withABF => withABF
                    .AddQueryString());

        builder.Services.AddHttpLogging(options =>
            options.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod | HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.RequestQuery);

        builder.Host.UseWindowsService(options =>
            options.ServiceName = Assembly.GetExecutingAssembly().GetName().Name);

        var app = builder.Build();

        app.UseHttpLogging();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}