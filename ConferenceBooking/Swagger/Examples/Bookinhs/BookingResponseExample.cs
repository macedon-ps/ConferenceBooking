using ConferenceBooking.Application.DTOs.Bookings;
using ConferenceBooking.Application.DTOs.Services;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Bookings;

/// <summary>
/// Клас BookingResponseExample, що надає приклад відповіді для бронювання.
/// </summary>
public class BookingResponseExample : IExamplesProvider<BookingResponse>
{
    /// <summary>
    /// Метод GetExamples, що повертає приклад відповіді для бронювання.
    /// </summary>
    /// <returns>Об'єкт BookingResponse з прикладом відповіді.</returns>
    public BookingResponse GetExamples()
    {
        return new BookingResponse
        {
            Id = SwaggerExampleData.BookingId_F,

            HallId = SwaggerExampleData.HallId_F,

            StartTime = SwaggerExampleData.StartTime,
                
            EndTime = SwaggerExampleData.EndTime,

            Services = new List<ServiceResponse>
            {
                new ServiceResponse
                {
                    Id = SwaggerExampleData.ProjectorId,
                    Name = "Проектор",
                    Price = 500
                },

                new ServiceResponse
                {
                    Id = SwaggerExampleData.WifiId,
                    Name = "Wi-Fi",
                    Price = 300
                },

                new ServiceResponse
                {
                    Id = SwaggerExampleData.SoundId,
                    Name = "Звук",
                    Price = 700
                }
            },

            TotalCost = 4150
        };
    }
}