using ConferenceBooking.Application.DTOs.Services;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Swagger.Examples.Services
{
    /// <summary>
    /// Клас ServiceResponseExample, що надає приклад відповіді для послуги.
    /// </summary>
    public class ServiceResponseExample : IExamplesProvider<ServiceResponse>
    {
        /// <summary>
        /// Метод GetExamples повертає приклад об'єкта ServiceResponse, що представляє послугу.
        /// </summary>
        /// <returns>Об'єкт ServiceResponse з прикладом відповіді.</returns>
        public ServiceResponse GetExamples()
        {
            return new ServiceResponse
            {
                Id = SwaggerExampleData.ProjectorId,
                Name = "Проектор",
                Price = 200
            };
        }
    }
}
