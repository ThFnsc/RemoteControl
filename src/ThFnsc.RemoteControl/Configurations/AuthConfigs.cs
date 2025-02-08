using ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;
using ThFnsc.RemoteControl.AuthHandlers.QueryString;

namespace ThFnsc.RemoteControl.Configurations;

public static class AuthConfigs
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(QueryStringAuthenticationDefaults.AuthenticationScheme)
            .UseAntiBruteforce(
                builderAction: withABF => withABF
                    .AddQueryString());

        builder.Services.AddAuthorization();
        return builder;
    }
}
