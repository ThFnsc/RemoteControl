using Microsoft.AspNetCore.Authentication;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

public static class QueryStringAuthenticationExtensions
{
    public static AuthenticationBuilder AddQueryString(this AuthenticationBuilder builder, string? authenticationScheme = null, Action<QueryStringAuthenticationOptions>? configure = null)
    {
        builder.Services.ConfigureSwaggerGen(sOptions =>
        {
            var conf = new QueryStringAuthenticationOptions();
            configure?.Invoke(conf);
            sOptions.OperationFilter<QueryStringAuthenticationActionParameterOperationFilter>(conf.QueryStringParameterName);
        });

        return builder
            .AddScheme<QueryStringAuthenticationOptions, QueryStringAuthenticationHandler>(
                authenticationScheme: authenticationScheme ?? QueryStringAuthenticationDefaults.AuthenticationScheme,
                configureOptions: configure);
    }
}
