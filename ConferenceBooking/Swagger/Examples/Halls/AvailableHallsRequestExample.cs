using ConferenceBooking.Application.DTOs.Halls;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Halls;

/// <summary>
/// Клас AvailableHallsRequestExample, що надає приклад запиту для отримання доступних залів.
/// </summary>
public sealed class AvailableHallsRequestExample : IExamplesProvider<AvailableHallsRequest>
{
    /// <summary>
    /// Метод GetExamples повертає приклад запиту AvailableHallsRequest з заповненими полями StartTime, EndTime та Capacity.
    /// </summary>
    /// <returns>Об'єкт AvailableHallsRequest з прикладом запиту для отримання доступних залів.</returns>
    public AvailableHallsRequest GetExamples()
    {
        return new AvailableHallsRequest
        {
            StartTime = SwaggerExampleData.StartTime,
            
            EndTime = SwaggerExampleData.EndTime,
            
            Capacity = 50
        };
    }
}