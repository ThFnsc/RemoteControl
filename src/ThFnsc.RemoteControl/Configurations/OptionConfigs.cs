using Microsoft.AspNetCore.Http.Json;
using ThFnsc.RemoteControl.Auth;
using ThFnsc.RemoteControl.Util;

namespace ThFnsc.RemoteControl.Configurations;

public static class OptionConfigs
{
    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("preferences.json", optional: false, reloadOnChange: true);

        builder.Services.AddOptions<QueryStringAuthenticationOptions>(QueryStringAuthenticationDefaults.AuthenticationScheme)
            .BindConfiguration(nameof(QueryStringAuthenticationOptions))
            .ValidateOnStart()
            .Validate(opt => builder.Environment.IsDevelopment() || opt.Token is not "changeme", "Access token is 'changeme', which is not allowed in production environments. Check the 'preferences.json' file")
            .Validate(opt => string.IsNullOrWhiteSpace(opt.QueryStringParameterName) is false, "QueryStringParameterName must have a value");

        builder.Services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.WriteIndented = true;
            opt.SerializerOptions.TypeInfoResolverChain.Add(BoolConverter.Default); // This is necessary for openapi to work. Don't ask me why.
        });

        return builder;
    }
}
