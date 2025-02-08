using ThFnsc.RemoteControl.Auth;

namespace ThFnsc.RemoteControl.Configurations;

public static class AuthConfigs
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(QueryStringAuthenticationDefaults.AuthenticationScheme)
            .AddQueryString();

        builder.Services.AddAuthorization();

        return builder;
    }
}
