using Microsoft.AspNetCore.Http.Json;
using ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;
using ThFnsc.RemoteControl.AuthHandlers.QueryString;

namespace ThFnsc.RemoteControl.Configurations;

public static class OptionConfigs
{
    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("preferences.json", optional: false, reloadOnChange: true);

        builder.Services.AddOptions<QueryStringAuthenticationOptions>(QueryStringAuthenticationDefaults.AuthenticationScheme)
            .BindConfiguration(nameof(QueryStringAuthenticationOptions))
            .ValidateOnStart()
            .Validate(opt => builder.Environment.IsDevelopment() || opt.Token is not "changeme", "Access token is 'changeme', which is not allowed in production environments. Check the 'preferences.json' file");

        builder.Services.AddOptions<AntiBruteforceOptions>()
            .BindConfiguration(nameof(AntiBruteforceOptions));

        builder.Services.Configure<JsonOptions>(opt => opt.SerializerOptions.WriteIndented = true);

        return builder;
    }
}
