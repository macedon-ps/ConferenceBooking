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
    public BookingsController(
        IBookingApplicationService bookingService)
    {
        _bookingService = bookingService;
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
}
