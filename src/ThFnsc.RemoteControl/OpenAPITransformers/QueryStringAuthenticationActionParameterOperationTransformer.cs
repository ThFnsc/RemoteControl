using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ThFnsc.RemoteControl.Auth;

namespace ThFnsc.RemoteControl.OpenAPITransformers;

public class QueryStringAuthenticationActionParameterOperationTransformer(IOptions<QueryStringAuthenticationOptions> options) : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var requiresAuthorization = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<AuthorizeAttribute>()
            .Any();

        if (requiresAuthorization is false)
            return Task.CompletedTask;

        var parameter = new OpenApiParameter()
        {
            AllowEmptyValue = false,
            Required = true,
            Description = "The authentication token set in the settings",
            Name = options.Value.QueryStringParameterName,
            In = ParameterLocation.Query,
            Example = new OpenApiString("changeme"),
            Schema = new()
            {
                Type = "string"
            }
        };

        (operation.Parameters ??= []).Add(parameter);

        return Task.CompletedTask;
    }
}
