using ConferenceBooking.Application.DTOs.Halls;
using ConferenceBooking.Application.DTOs.Services;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Halls
{
    /// <summary>
    /// Клас HallResponseExample, що надає приклад відповіді для об'єкта HallResponse.
    /// </summary>
    public class HallResponseExample : IExamplesProvider<HallResponse>
    {
        /// <summary>
        /// Метод GetExamples повертає приклад відповіді для об'єкта HallResponse.
        /// </summary>
        /// <returns>Об'єкт HallResponse з прикладом відповіді.</returns>
        public HallResponse GetExamples()
        {
            return new HallResponse
            {
                Id = SwaggerExampleData.HallId_A,
                Name = "Зал A",
                Capacity = 135,
                HourlyRate = 2200,

                Services = new List<ServiceResponse>
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
                        Price = 300
                    },

                    new ServiceResponse
                    {
                        Id = SwaggerExampleData.SoundId,
                        Name = "Звук",
                        Price = 700
                    }
                }
            };
        }
    }
}
