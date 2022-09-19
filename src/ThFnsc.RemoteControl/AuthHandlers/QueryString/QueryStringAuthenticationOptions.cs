using Microsoft.AspNetCore.Authentication;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

public class QueryStringAuthenticationOptions : AuthenticationSchemeOptions
{
    public string QueryStringParameterName { get; set; } = QueryStringAuthenticationDefaults.QueryStringParameterName;

    public string Token { get; set; } = null!;

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Token))
            throw new ArgumentNullException(nameof(Token), "Access token was not set");
        base.Validate();
    }
}
