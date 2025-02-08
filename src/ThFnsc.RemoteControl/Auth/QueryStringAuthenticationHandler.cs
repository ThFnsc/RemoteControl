using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ThFnsc.RemoteControl.Auth;

public class QueryStringAuthenticationHandler : AuthenticationHandler<QueryStringAuthenticationOptions>
{
    public QueryStringAuthenticationHandler(
        IOptionsMonitor<QueryStringAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (string.IsNullOrWhiteSpace(Options.Token))
            return AuthenticatedResultAsync();

        if (Context.Request.Query.TryGetValue(Options.QueryStringParameterName, out var values) is false)
            return Task.FromResult(AuthenticateResult.NoResult());

        if (values.Any(v => v == Options.Token))
            return AuthenticatedResultAsync();

        return Task.FromResult(AuthenticateResult.Fail($"Invalid authentication token"));
    }

    private Task<AuthenticateResult> AuthenticatedResultAsync()
    {
        var identity = new ClaimsIdentity(ClaimsIssuer);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, ClaimsIssuer);
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
