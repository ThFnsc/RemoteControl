using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ThFnsc.RemoteControl.AuthHandlers.QueryString;

internal class QueryStringAuthenticationActionParameterOperationFilter : IOperationFilter
{
    private readonly string _parameterName;

    public QueryStringAuthenticationActionParameterOperationFilter(string parameterName)
    {
        if(string.IsNullOrWhiteSpace(parameterName)) 
            throw new ArgumentNullException(nameof(parameterName));
        _parameterName = parameterName;
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
            Name = _parameterName,
            In = ParameterLocation.Query,
            Example = new OpenApiString("abc123"),
            Schema = new()
            {
                Type = "string"
            }
        });
    }
}
