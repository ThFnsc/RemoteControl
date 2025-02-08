using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

internal class QueryStringAuthenticationActionParameterOperationFilter : IOpenApiOperationTransformer
{
    private readonly IOptions<QueryStringAuthenticationOptions> _options;

    public QueryStringAuthenticationActionParameterOperationFilter(IOptions<QueryStringAuthenticationOptions> options)
    {
        _options = options;
        if (string.IsNullOrWhiteSpace(_options.Value.QueryStringParameterName))
            throw new ArgumentNullException(nameof(QueryStringAuthenticationOptions.QueryStringParameterName));
    }

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var requiresAuthorization = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<AuthorizeAttribute>()
            .Any();

        if (!requiresAuthorization)
            return Task.CompletedTask;

        (operation.Parameters ??= []).Add(new OpenApiParameter()
        {
            AllowEmptyValue = false,
            Required = true,
            Description = "The authentication token set in the settings",
            Name = _options.Value.QueryStringParameterName,
            In = ParameterLocation.Query,
            Example = new OpenApiString("abc123"),
            Schema = new()
            {
                Type = "string"
            }
        });
        return Task.CompletedTask;
    }
}
