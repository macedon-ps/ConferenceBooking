using ConferenceBooking.Api.Swagger.Examples.Halls;
using ConferenceBooking.Application.DTOs.Halls;
using ConferenceBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ConferenceBooking.Api.Controllers;

/// <summary>
/// Клас контролера для управління залами конференцій. Реалізує REST API для створення, оновлення, видалення та отримання доступних залів.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallsController : ControllerBase
{
    /// <summary>
    /// Сервіс для управління залами конференцій. Використовується для обробки запитів на створення, оновлення, видалення та отримання доступних залів.
    /// </summary>
    private readonly IHallApplicationService _hallService;

    /// <summary>
    /// Конструктор класу HallsController, який приймає сервіс для управління залами конференцій як параметр. Ініціалізує приватне поле для доступу до методів сервісу.
    /// </summary>
    /// <param name="hallService">Сервіс для управління залами конференцій.</param>
    public HallsController(IHallApplicationService hallService)
    {
        _hallService = hallService;
    }

    /// <summary>
    /// Отримати список всіх залів.
    /// </summary>
    /// <remarks>Метод для отримання списку всіх залів. Викликає сервіс для отримання даних та повертає колекцію об'єктів HallResponse з інформацією про всі зали.
    /// </remarks>
    /// <returns>Колекція об'єктів HallResponse з інформацією про всі зали.</returns>
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HallsResponseExample))]
    public async Task<ActionResult<IReadOnlyCollection<HallResponse>>> GetAll()
    {
        var halls = await _hallService.GetAllAsync();

        return Ok(halls);
    }

    /// <summary>
    /// Отримати зал за його ідентифікатором.
    /// </summary>
    /// <remarks>Метод для отримання конкретного зал за його ідентифікатором. Викликає сервіс для отримання даних та повертає об'єкт HallResponse з інформацією про зал.
    /// </remarks>
    /// <param name="id">Ідентифікатор залу, який потрібно отримати.</param>
    /// <returns>Об'єкт HallResponse з інформацією про зал.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HallResponseExample))]
    public async Task<ActionResult<HallResponse>> GetById(Guid id)
    {
        var hall = await _hallService.GetByIdAsync(id);

        return Ok(hall);
    }

    /// <summary>
    /// Отримати список доступних залів на основі заданих параметрів.
    /// </summary>
    /// <remarks>Метод для отримання списку доступних залів на основі заданих параметрів. Приймає об'єкт AvailableHallsRequest, який містить параметри пошуку, та повертає колекцію об'єктів HallResponse з інформацією про доступні зали.
    /// </remarks>
    /// <param name="request">Об'єкт AvailableHallsRequest, який містить параметри пошуку доступних залів.</param>
    /// <returns>Колекція об'єктів HallResponse з інформацією про доступні зали.</returns>
    [HttpGet("available")]
    [SwaggerRequestExample(typeof(AvailableHallsRequest), typeof(AvailableHallsRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HallsResponseExample))]
    public async Task<ActionResult<IReadOnlyCollection<HallResponse>>> GetAvailable([FromQuery] AvailableHallsRequest request)
    {
        var response = await _hallService.GetAvailableAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Створити новий зал.
    /// </summary>
    /// <remarks>Метод для створення нового залу. Приймає об'єкт CreateHallRequest, який містить дані для створення залу, та повертає об'єкт HallResponse з інформацією про створений зал.
    /// </remarks>
    /// <param name="request">Об'єкт CreateHallRequest, який містить дані для створення залу.</param>
    /// <returns>Об'єкт HallResponse з інформацією про створений зал.</returns>
    [HttpPost]
    [SwaggerRequestExample(typeof(CreateHallRequest), typeof(CreateHallRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HallResponseExample))]
    public async Task<ActionResult<HallResponse>> Create(CreateHallRequest request)
    {
        var response = await _hallService.CreateAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Оновити існуючий зал.
    /// </summary>
    /// <remarks>Метод для оновлення існуючого залу. Приймає ідентифікатор залу та об'єкт UpdateHallRequest, який містить нові дані для оновлення залу, та повертає об'єкт HallResponse з оновленою інформацією про зал.
    /// </remarks>
    /// <param name="id">Ідентифікатор залу, який потрібно оновити.</param>
    /// <param name="request">Об'єкт UpdateHallRequest, який містить нові дані для оновлення залу.</param>
    /// <returns>Об'єкт HallResponse з оновленою інформацією про зал.</returns>
    [HttpPut("{id:guid}")]
    [SwaggerRequestExample(typeof(UpdateHallRequest), typeof(UpdateHallRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HallResponseExample))]
    public async Task<ActionResult<HallResponse>> Update(Guid id, UpdateHallRequest request)
    {
        var response = await _hallService.UpdateAsync(id, request);

        return Ok(response);
    }

    /// <summary>
    /// Видалити існуючий зал.
    /// </summary>
    /// <remarks>Метод для видалення існуючого залу. Приймає ідентифікатор залу, який потрібно видалити, та повертає статус NoContent у разі успішного видалення.
    /// </remarks>
    /// <param name="id">Ідентифікатор залу, який потрібно видалити.</param>
    /// <returns>Статус NoContent у разі успішного видалення.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _hallService.DeleteAsync(id);

        return NoContent();
    }
}
