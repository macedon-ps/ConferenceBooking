using ConferenceBooking.Application.DTOs.Bookings;
using ConferenceBooking.Application.DTOs.Services;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Bookinhs
{
    /// <summary>
    /// Клас BookingsResponseExample, що надає приклади відповідей для Swagger документації для колекції бронювань.
    /// </summary>
    public class BookingsResponseExample : IExamplesProvider<IEnumerable<BookingResponse>>
    {
        /// <summary>
        /// Метод GetExamples, що повертає приклади відповідей для Swagger документації.
        /// </summary>
        /// <returns>Колекція об'єктів BookingResponse з прикладами відповідей.</returns>
        public IEnumerable<BookingResponse> GetExamples()
        {
            return new List<BookingResponse>
            {
                new BookingResponse
                {
                    Id = SwaggerExampleData.BookingId_C,
                    HallId = SwaggerExampleData.HallId_C,

                    StartTime = SwaggerExampleData.StartTime,
                    EndTime = SwaggerExampleData.EndTime,

                    TotalCost = 3700,

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
                },
                new BookingResponse
                {
                    Id = SwaggerExampleData.BookingId_F,
                    HallId = SwaggerExampleData.HallId_F,

                    StartTime = SwaggerExampleData.StartTime2,
                    EndTime = SwaggerExampleData.EndTime2,

                    TotalCost = 4500,

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
                }
            };
        }
    }
}
