using System.Reflection;

namespace ThFnsc.RemoteControl.Configurations;

public static class ServiceConfigs
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter();

        builder.Host.UseWindowsService(options =>
            options.ServiceName = Assembly.GetExecutingAssembly().GetName().Name!);
        
        return builder;
    }
}
