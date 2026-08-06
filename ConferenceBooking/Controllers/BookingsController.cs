using ConferenceBooking.Application.DTOs.Bookings;
using ConferenceBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers;

/// <summary>
/// Клас контролера для управління бронюваннями конференцій. Реалізує REST API для створення нових бронювань та обробки запитів, пов'язаних з бронюваннями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    /// <summary>
    /// Сервіс для управління бронюваннями конференцій. Використовується для обробки запитів на створення нових бронювань та перевірки конфліктів у часі.
    /// </summary>
    private readonly IBookingApplicationService _bookingService;

    /// <summary>
    /// Конструктор класу BookingsController, який приймає сервіс для управління бронюваннями конференцій як параметр. Ініціалізує приватне поле для доступу до методів сервісу.
    /// </summary>
    /// <param name="bookingService">Сервіс для управління бронюваннями конференцій.</param>
    public BookingsController(IBookingApplicationService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// Метод для отримання всіх бронювань конференцій. Викликає сервіс для отримання списку всіх бронювань та повертає їх у вигляді колекції об'єктів BookingResponse.
    /// </summary>
    /// <returns>Колекція об'єктів BookingResponse, що представляють всі бронювання конференцій.</returns>
    [HttpGet] 
    public async Task<ActionResult<IReadOnlyCollection<BookingResponse>>> GetAll() 
    { 
        var bookings = await _bookingService.GetAllAsync(); 
        return Ok(bookings); 
    }

    /// <summary>
    /// Метод для отримання конкретного бронювання конференції за його унікальним ідентифікатором. Викликає сервіс для отримання бронювання за вказаним Guid id та повертає об'єкт BookingResponse з деталями бронювання.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання конференції.</param>
    /// <returns>Об'єкт BookingResponse з деталями бронювання конференції.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookingResponse>> GetById(Guid id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        return Ok(booking);
    }

    /// <summary>
    /// Метод для отримання всіх бронювань конференцій, пов'язаних з конкретним залом. Викликає сервіс для отримання списку бронювань за вказаним Guid hallId та повертає їх у вигляді колекції об'єктів BookingResponse.
    /// </summary>
    /// <param name="hallId">Унікальний ідентифікатор залу.</param>
    /// <returns>Колекція об'єктів BookingResponse, що представляють всі бронювання конференцій для вказаного залу.</returns>
    [HttpGet("by-hall/{hallId:guid}")]
    public async Task<ActionResult<IReadOnlyCollection<BookingResponse>>> GetByHall(Guid hallId)
    {
        var bookings = await _bookingService.GetByHallAsync(hallId);

        return Ok(bookings);
    }


    /// <summary>
    /// Метод для створення нового бронювання конференції. Приймає об'єкт CreateBookingRequest, який містить інформацію про бронювання, та повертає об'єкт BookingResponse з деталями створеного бронювання.
    /// </summary>
    /// <param name="request">Об'єкт CreateBookingRequest, який містить інформацію про бронювання.</param>
    /// <returns>Об'єкт BookingResponse з деталями створеного бронювання.</returns>
    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request)
    {
        var response = await _bookingService.CreateAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Метод для видалення існуючого бронювання конференції за його унікальним ідентифікатором. Приймає Guid id бронювання, яке потрібно видалити, та повертає статус NoContent у разі успішного видалення.
    /// </summary>
    /// <param name="id">Ідентифікатор бронювання, яке потрібно видалити</param>
    /// <returns>Статус NoContent у разі успішного видалення</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _bookingService.DeleteAsync(id);

        return NoContent();
    }
}
