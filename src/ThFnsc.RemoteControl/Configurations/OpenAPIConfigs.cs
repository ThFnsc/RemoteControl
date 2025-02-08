using ThFnsc.RemoteControl.Auth;

namespace ThFnsc.RemoteControl.Configurations;

public static class OpenAPIConfigs
{
    public static WebApplicationBuilder AddOpenAPI(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(conf => conf.AddOperationTransformer<QueryStringAuthenticationActionParameterOperationFilter>());
        return builder;
    }
}