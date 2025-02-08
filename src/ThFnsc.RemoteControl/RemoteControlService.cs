using Microsoft.Extensions.Hosting.WindowsServices;
using Swashbuckle.AspNetCore.SwaggerUI;
using ThFnsc.RemoteControl.Configurations;
using ThFnsc.RemoteControl.Util;

namespace ThFnsc.RemoteControl;

public class RemoteControlService
{
    public static Task StartServiceAsync(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
        });

        builder
            .AddOptions()
            .AddCustomLogging()
            .AddOpenAPI()
            .AddAuth()
            .AddRateLimiting()
            .AddServices();

        var app = builder.Build();

        app.UseStatusCodePages();

        app.UseRateLimiter();

        app.UseAuthentication();
        
        app.UseAuthorization();

        app.MapOpenApi();

        app.UseSwaggerUI(new SwaggerUIOptions
        {
            ConfigObject = new()
            {
                Urls = [new() { Url = "/openapi/v1.json", Name = "v1" }]
            }
        });

        app.MapGet("/Ping", () => "Pong!");

        var group = app.MapGroup("Power")
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitConfigs.DefaultPolicy);
        group.MapGet("/Shutdown", PowerUtils.ShutdownAsync);
        group.MapGet("/Lock", PowerUtils.LockAsync);
        group.MapGet("/Abort", PowerUtils.AbortAsync);
        group.MapGet("/Reboot", PowerUtils.RebootAsync);
        group.MapGet("/Logoff", PowerUtils.LogoffAsync);
        group.MapGet("/Sleep", PowerUtils.SleepAsync);

        return app.RunAsync();
    }
}