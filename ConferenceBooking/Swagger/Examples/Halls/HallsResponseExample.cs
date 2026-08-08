using ConferenceBooking.Application.DTOs.Halls;
using ConferenceBooking.Application.DTOs.Services;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Halls
{
    /// <summary>
    /// Клас HallsResponseExample, що надає приклад відповіді для списку залів.
    /// </summary>
    public class HallsResponseExample : IExamplesProvider<IEnumerable<HallResponse>>
    {
        /// <summary>
        /// Метод GetExamples повертає приклад відповіді для списку залів.
        /// </summary>
        /// <returns>Колекція об'єктів HallResponse з прикладами відповідей.</returns>
        public IEnumerable<HallResponse> GetExamples()
        {
            return new List<HallResponse>
            {
                new HallResponse
                {
                    Id = SwaggerExampleData.HallId_A,
                    Name = "Малый конференц-зал",
                    Capacity = 50,
                    HourlyRate = 1500,

                    Services = new List<ServiceResponse>
                    {
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.SoundId,
                            Name = "Звуковая система",
                            Price = 500
                        },
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.ProjectorId,
                            Name = "Проектор",
                            Price = 1000
                        },
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.WifiId,
                            Name = "Wi-Fi",
                            Price = 200
                        }
                    }
                },
                new HallResponse
                {
                    Id = SwaggerExampleData.HallId_B,
                    Name = "Средний конференц-зал",
                    Capacity = 100,
                    HourlyRate = 2500,

                    Services = new List<ServiceResponse>
                    {
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.SoundId,
                            Name = "Звуковая система",
                            Price = 500
                        },
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.ProjectorId,
                            Name = "Проектор",
                            Price = 1000
                        },
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.WifiId,
                            Name = "Wi-Fi",
                            Price = 200
                        }
                    }
                },
                new HallResponse
                {
                    Id = SwaggerExampleData.HallId_C,
                    Name = "Большой конференц-зал",
                    Capacity = 200,
                    HourlyRate = 4000,

                    Services = new List<ServiceResponse>
                    {
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.SoundId,
                            Name = "Звуковая система",
                            Price = 500
                        },
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.ProjectorId,
                            Name = "Проектор",
                            Price = 1000
                        },
                        new ServiceResponse
                        {
                            Id = SwaggerExampleData.WifiId,
                            Name = "Wi-Fi",
                            Price = 200
                        }
                    }
                }
            };
        }
    }
}
