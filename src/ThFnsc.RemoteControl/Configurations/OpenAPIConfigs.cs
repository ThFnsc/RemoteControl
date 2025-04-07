using ThFnsc.RemoteControl.OpenAPITransformers;

namespace ThFnsc.RemoteControl.Configurations;

public static class OpenAPIConfigs
{
    public static WebApplicationBuilder AddOpenAPI(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(conf =>
        {
            conf.AddOperationTransformer<QueryStringAuthenticationActionParameterOperationTransformer>();
            conf.AddDocumentTransformer<SetServerToBaseAddressDocumentTransformer>();
        });

        return builder;
    }
}
