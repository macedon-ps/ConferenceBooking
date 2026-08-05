using ConferenceBooking.Application.DTOs.Halls;
using ConferenceBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    /// Метод для створення нового залу конференцій. Приймає об'єкт CreateHallRequest, який містить дані для створення залу, та повертає об'єкт HallResponse з інформацією про створений зал.
    /// </summary>
    /// <param name="request">Об'єкт CreateHallRequest, який містить дані для створення залу.</param>
    /// <returns>Об'єкт HallResponse з інформацією про створений зал.</returns>
    [HttpPost]
    public async Task<ActionResult<HallResponse>> Create(CreateHallRequest request)
    {
        var response = await _hallService.CreateAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Метод для оновлення існуючого залу конференцій. Приймає ідентифікатор залу та об'єкт UpdateHallRequest, який містить нові дані для оновлення залу, та повертає об'єкт HallResponse з оновленою інформацією про зал.
    /// </summary>
    /// <param name="id">Ідентифікатор залу, який потрібно оновити.</param>
    /// <param name="request">Об'єкт UpdateHallRequest, який містить нові дані для оновлення залу.</param>
    /// <returns>Об'єкт HallResponse з оновленою інформацією про зал.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<HallResponse>> Update(Guid id, UpdateHallRequest request)
    {
        var response = await _hallService.UpdateAsync(id, request);

        return Ok(response);
    }

    /// <summary>
    /// Метод для видалення існуючого залу конференцій. Приймає ідентифікатор залу, який потрібно видалити, та повертає статус NoContent у разі успішного видалення.
    /// </summary>
    /// <param name="id">Ідентифікатор залу, який потрібно видалити.</param>
    /// <returns>Статус NoContent у разі успішного видалення.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _hallService.DeleteAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Метод для отримання списку доступних залів конференцій на основі заданих параметрів. Приймає об'єкт AvailableHallsRequest, який містить параметри пошуку, та повертає колекцію об'єктів HallResponse з інформацією про доступні зали.
    /// </summary>
    /// <param name="request">Об'єкт AvailableHallsRequest, який містить параметри пошуку доступних залів.</param>
    /// <returns>Колекція об'єктів HallResponse з інформацією про доступні зали.</returns>
    [HttpGet("available")]
    public async Task<ActionResult<IReadOnlyCollection<HallResponse>>> GetAvailable([FromQuery] AvailableHallsRequest request)
    {
        var response = await _hallService.GetAvailableAsync(request);

        return Ok(response);
    }
}
