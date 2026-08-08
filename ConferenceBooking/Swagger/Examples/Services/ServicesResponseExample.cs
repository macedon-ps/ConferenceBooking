using ConferenceBooking.Application.DTOs.Services;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Services
{
    /// <summary>
    /// Клас ServicesResponseExample, що надає приклад відповіді для списку послуг.
    /// </summary>
    public class ServicesResponseExample : IExamplesProvider<IEnumerable<ServiceResponse>>
    {
        /// <summary>
        /// Метод GetExamples повертає приклад списку об'єктів ServiceResponse, що представляють послуги.
        /// </summary>
        /// <returns>Колекція об'єктів ServiceResponse з прикладами послуг.</returns>
        public IEnumerable<ServiceResponse> GetExamples()
        {
            return new List<ServiceResponse>
            {
                new ServiceResponse
                {
                    Id = SwaggerExampleData.ProjectorId,
                    Name = "Проектор",
                    Price = 200
                },
                new ServiceResponse
                {
                    Id = SwaggerExampleData.WifiId,
                    Name = "Wi-Fi",
                    Price = 100
                },
                new ServiceResponse
                {
                    Id = SwaggerExampleData.SoundId,
                    Name = "Звуковая система",
                    Price = 300
                }
            };
        }
    }
}
