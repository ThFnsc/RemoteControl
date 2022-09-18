using Microsoft.AspNetCore.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;

public class AntiBruteforceAuthenticationBuilder : AuthenticationBuilder
{
    public AntiBruteforceAuthenticationBuilder(AuthenticationBuilder builder) : base(builder.Services) { }

    public override AuthenticationBuilder AddScheme<TOptions, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(string authenticationScheme, string? displayName, Action<TOptions>? configureOptions)
    {
        Services.Configure<AuthenticationOptions>(o =>
            o.AddScheme(authenticationScheme, scheme =>
                {
                    scheme.HandlerType = typeof(AntiBruteforceWrapper<THandler>);
                    scheme.DisplayName = displayName;
                }));
        if (configureOptions != null)
        {
            Services.Configure(authenticationScheme, configureOptions);
        }
        Services.AddOptions<TOptions>(authenticationScheme).Validate(o =>
        {
            o.Validate(authenticationScheme);
            return true;
        });
        Services.AddTransient<AntiBruteforceWrapper<THandler>>();
        Services.AddTransient<THandler>();
        return this;
    }
}