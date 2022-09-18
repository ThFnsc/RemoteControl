using Microsoft.AspNetCore.Authentication;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

public static class QueryStringAuthenticationExtensions
{
    public static AuthenticationBuilder AddQueryString(this AuthenticationBuilder builder, string? authenticationScheme = null, Action<QueryStringAuthenticationOptions>? configure = null)
    {
        builder.Services.ConfigureSwaggerGen(sOptions =>
        {
            if (configure != null)
                builder.Services.Configure(configure);
            sOptions.OperationFilter<QueryStringAuthenticationActionParameterOperationFilter>();
        });

        return builder
            .AddScheme<QueryStringAuthenticationOptions, QueryStringAuthenticationHandler>(
                authenticationScheme: authenticationScheme ?? QueryStringAuthenticationDefaults.AuthenticationScheme,
                configureOptions: configure);
    }
}
