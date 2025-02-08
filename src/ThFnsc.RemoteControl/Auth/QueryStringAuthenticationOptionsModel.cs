namespace ThFnsc.RemoteControl.Auth;

public class QueryStringAuthenticationOptionsModel
{
    public string QueryStringParameterName { get; set; } = QueryStringAuthenticationDefaults.QueryStringParameterName;

    public string? Token { get; set; }
}