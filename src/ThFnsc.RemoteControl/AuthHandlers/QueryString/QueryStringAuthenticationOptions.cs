using Microsoft.AspNetCore.Authentication;
using ThFnsc.RemoteControl.Util;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

public class QueryStringAuthenticationOptions : AuthenticationSchemeOptions
{
    public string QueryStringParameterName { get; set; } = QueryStringAuthenticationDefaults.QueryStringParameterName;

    public string Token { get; set; } = null!;

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Token))
            throw new TokenMissingException("Access token was not set. Check the 'preferences.json' file");
        base.Validate();
    }
}
