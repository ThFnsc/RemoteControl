using Microsoft.AspNetCore.Authentication;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

public static class QueryStringAuthenticationExtensions
{
    public static AuthenticationBuilder AddQueryString(this AuthenticationBuilder builder, string? authenticationScheme = null, Action<QueryStringAuthenticationOptions>? configure = null)
    {
        if (configure != null)
            builder.Services.Configure(configure);

        return builder
            .AddScheme<QueryStringAuthenticationOptions, QueryStringAuthenticationHandler>(
                authenticationScheme: authenticationScheme ?? QueryStringAuthenticationDefaults.AuthenticationScheme,
                configureOptions: configure);
    }
}
