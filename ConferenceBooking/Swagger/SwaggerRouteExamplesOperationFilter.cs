using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ConferenceBooking.Api.Swagger;

/// <summary>
/// Клас фільтра операцій Swagger, який додає приклади значень для параметрів маршруту, що мають формат UUID. Використовується для покращення документації Swagger, забезпечуючи наочні приклади для користувачів API.
/// </summary>
public class SwaggerRouteExamplesOperationFilter : IOperationFilter
{
    /// <summary>
    /// Метод, який застосовує фільтр до операції Swagger. Перевіряє параметри операції та додає приклади значень для параметрів маршруту, що мають формат UUID.
    /// </summary>
    /// <param name="operation">Операція Swagger, до якої застосовується фільтр.</param>
    /// <param name="context">Контекст фільтра операції Swagger.</param>
    public void Apply(
    OpenApiOperation operation,
    OperationFilterContext context)
    {
        var relativePath =
            context.ApiDescription.RelativePath?
                .ToLowerInvariant();

        foreach (var parameter in operation.Parameters)
        {
            // Path parameters
            if (parameter.In == ParameterLocation.Path)
            {
                parameter.Example = GetPathExample(relativePath);
            }

            // Query parameters
            if (parameter.In == ParameterLocation.Query)
            {
                parameter.Example =
                    GetQueryExample(relativePath, parameter.Name);
            }
        }
    }

    /// <summary>
    /// Метод, який повертає приклад значення для параметра маршруту на основі відносного шляху API. Використовується для надання конкретних прикладів значень UUID для різних маршрутів.
    /// </summary>
    /// <param name="relativePath">Відносний шлях API, для якого потрібно отримати приклад значення параметра маршруту.</param>
    /// <returns>Приклад значення параметра маршруту у форматі IOpenApiAny.</returns>
    private static IOpenApiAny GetPathExample(
        string? relativePath)
    {
        return relativePath?.ToLowerInvariant() switch
        {
            "api/halls/{id}" =>
                new OpenApiString(SwaggerExampleData.HallId_A.ToString()),

            "api/services/{id}" =>
                new OpenApiString(SwaggerExampleData.ProjectorId.ToString()),

            "api/bookings/{id}" =>
                new OpenApiString(SwaggerExampleData.BookingId_C.ToString()),

            "api/bookings/byhall/{hallid}" =>
                new OpenApiString(SwaggerExampleData.HallId_C.ToString()),

            _ =>
                new OpenApiString(Guid.Empty.ToString())
        };
    }

   /// <summary>
   /// Метод який повертає приклад значень для вхідних параметрів.
   /// </summary>
   /// <param name="relativePath">Відносинй шлях API.</param>
   /// <param name="parameterName">Назва параметру.</param>
   /// <returns></returns>
    private static IOpenApiAny? GetQueryExample(string? relativePath, string? parameterName)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || string.IsNullOrWhiteSpace(parameterName))
        {
            return null;
        }

        relativePath = relativePath.ToLowerInvariant();
        parameterName = parameterName.ToLowerInvariant();

        // Halls / available
        if (relativePath == "api/halls/available")
        {
            return parameterName switch
            {
                "starttime" =>
                new OpenApiString(SwaggerExampleData.StartTime.ToString("yyyy-MM-ddTHH:mm:ss")),

                "endtime" =>
                    new OpenApiString(SwaggerExampleData.EndTime.ToString("yyyy-MM-ddTHH:mm:ss")),

                "capacity" =>
                    new OpenApiInteger(SwaggerExampleData.Capacity),

                _ => null
            };
        }

        // Reports
        if (relativePath.StartsWith("api/reports/"))
        {
            return parameterName switch
            {
                "from" =>
                    new OpenApiString(SwaggerExampleData.ReportFrom.ToString("yyyy-MM-ddTHH:mm:ss")),

                "to" =>
                    new OpenApiString(
                        SwaggerExampleData.ReportTo.ToString("yyyy-MM-ddTHH:mm:ss")),

                _ => null
            };
        }

        return null;
    }
}