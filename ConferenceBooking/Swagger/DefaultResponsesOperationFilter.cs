using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ConferenceBooking.Api.Swagger;

public sealed class DefaultResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation,
        OperationFilterContext context)
    {
        AddResponse(operation, "400", "Bad Request");
        AddResponse(operation, "404", "Resource not found");
        AddResponse(operation, "409", "Conflict");
        AddResponse(operation, "500", "Internal server error");
    }

    private static void AddResponse(
        OpenApiOperation operation,
        string statusCode,
        string description)
    {
        if (!operation.Responses.ContainsKey(statusCode))
        {
            operation.Responses.Add(statusCode,
                new OpenApiResponse
                {
                    Description = description
                });
        }
    }
}
