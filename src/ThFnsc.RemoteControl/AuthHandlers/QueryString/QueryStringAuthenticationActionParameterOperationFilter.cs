using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

internal class QueryStringAuthenticationActionParameterOperationFilter : IOperationFilter
{
    private readonly IOptions<QueryStringAuthenticationOptions> _options;

    public QueryStringAuthenticationActionParameterOperationFilter(IOptions<QueryStringAuthenticationOptions> options)
    {
        _options = options;
        if (string.IsNullOrWhiteSpace(_options.Value.QueryStringParameterName))
            throw new ArgumentNullException(nameof(QueryStringAuthenticationOptions.QueryStringParameterName));
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requiresAuthorization = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<AuthorizeAttribute>()
            .Any();

        if (!requiresAuthorization)
            return;

        operation.Parameters.Add(new OpenApiParameter()
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
    }
}
