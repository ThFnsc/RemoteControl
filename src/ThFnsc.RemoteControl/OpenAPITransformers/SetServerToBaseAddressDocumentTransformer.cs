using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace ThFnsc.RemoteControl.OpenAPITransformers;

public class SetServerToBaseAddressDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Servers = [new() { Url = "/" }];
        return Task.CompletedTask;
    }
}