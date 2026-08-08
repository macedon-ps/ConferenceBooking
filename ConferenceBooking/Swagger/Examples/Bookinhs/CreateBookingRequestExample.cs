using ConferenceBooking.Application.DTOs.Bookings;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Bookinhs;

/// <summary>
/// Клас CreateBookingRequestExample, що надає приклад запиту для створення бронювання.
/// </summary>
public class CreateBookingRequestExample : IExamplesProvider<CreateBookingRequest>
{
    /// <summary>
    /// Метод GetExamples повертає приклад запиту для створення бронювання.
    /// </summary>
    /// <returns>Об'єкт CreateBookingRequest з прикладом запиту для створення бронювання.</returns>
    public CreateBookingRequest GetExamples()
    {
        return new CreateBookingRequest
        {
            HallId = SwaggerExampleData.HallId_F,

            StartTime = SwaggerExampleData.StartTime,

            EndTime = SwaggerExampleData.EndTime,

            ServiceIds =
            [
                SwaggerExampleData.ProjectorId,
                SwaggerExampleData.WifiId,
                SwaggerExampleData.SoundId
            ]
        };
    }
}