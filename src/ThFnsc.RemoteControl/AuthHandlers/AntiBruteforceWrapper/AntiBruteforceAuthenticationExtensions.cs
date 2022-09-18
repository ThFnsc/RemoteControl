using Microsoft.AspNetCore.Authentication;

namespace ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;

public static class AntiBruteforceAuthenticationExtensions
{
    public static AuthenticationBuilder UseAntiBruteforce(
        this AuthenticationBuilder builder,
        Action<AntiBruteforceAuthenticationBuilder> builderAction,
        Action<AntiBruteforceOptions>? configureOptions = null)
    {
        builder.Services.AddOptions<AntiBruteforceOptions>();
        if (configureOptions != null)
            builder.Services.Configure(configureOptions);
        builderAction(new AntiBruteforceAuthenticationBuilder(builder));
        return builder;
    }
}
