using Microsoft.AspNetCore.Mvc;
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
            .AddServices();

        var app = builder.Build();

        app.UseStatusCodePages();

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

        var group = app.MapGroup("Power").RequireAuthorization();
        //group.MapGet("/Shutdown", ([FromServices] ILogger<RemoteControlService> logger, bool hybrid, int seconds) => PowerUtils.ShutdownAsync(logger, hybrid, seconds));
        group.MapGet("/Lock", PowerUtils.LockAsync);
        group.MapGet("/Abort", PowerUtils.AbortAsync);
        group.MapGet("/Reboot", PowerUtils.RebootAsync);
        group.MapGet("/Logoff", PowerUtils.LogoffAsync);
        group.MapGet("/Sleep", PowerUtils.SleepAsync);

        return app.RunAsync();
    }
}