using ConferenceBooking.Application.DTOs.Halls;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Halls;

/// <summary>
/// Клас UpdateHallRequestExample, що надає приклад запиту для оновлення інформації про зал.
/// </summary>
public sealed class UpdateHallRequestExample : IExamplesProvider<UpdateHallRequest>
{
    /// <summary>
    /// Метод GetExamples повертає приклад запиту для оновлення інформації про зал. 
    /// </summary>
    /// <returns>Об'єкт UpdateHallRequest з прикладом запиту для оновлення інформації про зал.</returns>
    public UpdateHallRequest GetExamples()
    {
        return new UpdateHallRequest
        {
            Name = "Большой конференц-зал (обновленный)",

            Capacity = 200,

            HourlyRate = 4200,

            ServiceIds =
            [
                SwaggerExampleData.SoundId,
                SwaggerExampleData.WifiId,
                SwaggerExampleData.ProjectorId
            ]
        };
    }
}
