using Microsoft.AspNetCore.Authentication;

namespace ThFnsc.RemoteControl.Auth;

public class QueryStringAuthenticationOptions : AuthenticationSchemeOptions
{
    public string QueryStringParameterName { get; set; } = QueryStringAuthenticationDefaults.QueryStringParameterName;

    public string? Token { get; set; }
}
