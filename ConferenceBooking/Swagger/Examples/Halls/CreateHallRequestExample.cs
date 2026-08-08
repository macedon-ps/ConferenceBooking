using ConferenceBooking.Application.DTOs.Halls;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Halls;

/// <summary>
/// Клас CreateHallRequestExample, що надає приклад запиту для створення нового залу.
/// </summary>
public sealed class CreateHallRequestExample : IExamplesProvider<CreateHallRequest>
{
    /// <summary>
    /// Метод GetExamples повертає приклад запиту для створення нового залу.
    /// </summary>
    /// <returns>Об'єкт CreateHallRequest з прикладом запиту для створення нового залу.</returns>
    public CreateHallRequest GetExamples()
    {
        return new CreateHallRequest
        {
            Name = "Большой конференц-зал",

            Capacity = 180,

            HourlyRate = 3800,

            ServiceIds =
            [
                SwaggerExampleData.SoundId,
                SwaggerExampleData.WifiId,
                SwaggerExampleData.ProjectorId
            ]
        };
    }
}