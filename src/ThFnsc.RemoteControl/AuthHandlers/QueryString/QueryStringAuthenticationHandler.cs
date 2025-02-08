using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

public class QueryStringAuthenticationHandler : AuthenticationHandler<QueryStringAuthenticationOptions>
{
    public QueryStringAuthenticationHandler(
        IOptionsMonitor<QueryStringAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Query.TryGetValue(Options.QueryStringParameterName, out var values))
        {
            if (values.Any(v => v == Options.Token))
            {
                var identity = new ClaimsIdentity(ClaimsIssuer);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, ClaimsIssuer);
                var result = AuthenticateResult.Success(ticket);
                return Task.FromResult(result);
            }
            else
                return Task.FromResult(AuthenticateResult.Fail($"Invalid authentication token"));
        }
        else
            return Task.FromResult(AuthenticateResult.NoResult());
    }
}
