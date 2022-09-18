using Microsoft.AspNetCore.Authentication;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

public class QueryStringAuthenticationOptions : AuthenticationSchemeOptions
{
    public string QueryStringParameterName { get; set; } = QueryStringAuthenticationDefaults.QueryStringParameterName;

    public string? Token { get; set; }
}
