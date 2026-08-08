using ConferenceBooking.Api.Swagger.Examples.Services;
using ConferenceBooking.Application.DTOs.Services;
using ConferenceBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Controllers;

/// <summary>
/// Клас контролера для управління послугами конференцій. Реалізує REST API для отримання списку всіх послуг та отримання конкретної послуги за її унікальним ідентифікатором.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    /// <summary>
    /// Сервіс для управління послугами конференцій. Використовується для обробки запитів на отримання списку всіх послуг та отримання конкретної послуги за її унікальним ідентифікатором.
    /// </summary>
    private readonly IServiceApplicationService _serviceApplicationService;

    /// <summary>
    /// Конструктор класу ServicesController, який приймає сервіс для управління послугами конференцій як параметр. Ініціалізує приватне поле для доступу до методів сервісу.
    /// </summary>
    /// <param name="serviceApplicationService">Сервіс для управління послугами конференцій.</param>
    public ServicesController(
        IServiceApplicationService serviceApplicationService)
    {
        _serviceApplicationService = serviceApplicationService;
    }

    /// <summary>
    /// Отримати список всіх послуг.
    /// </summary>
    /// <remarks>Метод для отримання списку всіх послуг. Викликає сервіс для отримання даних та повертає колекцію об'єктів ServiceResponse з інформацією про всі послуги.
    /// </remarks>
    /// <returns>Колекція об'єктів ServiceResponse з інформацією про всі послуги.</returns>
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ServicesResponseExample))]
    public async Task<ActionResult<IReadOnlyCollection<ServiceResponse>>> GetAll()
    {
        var services = await _serviceApplicationService.GetAllAsync();

        return Ok(services);
    }

    /// <summary>
    /// Отримати послугу за її унікальним ідентифікатором.
    /// </summary>
    /// <remarks>Метод для отримання інформації про конкретну послугу залу за її унікальним ідентифікатором. Викликає сервіс для отримання даних та повертає об'єкт ServiceResponse з інформацією про послугу.
    /// </remarks>
    /// <param name="id">Унікальний ідентифікатор послуги.</param>
    /// <returns>Об'єкт ServiceResponse з інформацією про послугу.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ServiceResponseExample))]
    public async Task<ActionResult<ServiceResponse>> GetById(Guid id)
    {
        var service = await _serviceApplicationService.GetByIdAsync(id);

        return Ok(service);
    }
}
