using ConferenceBooking.Api.Models;
using ConferenceBooking.Application.DTOs.Reports;
using ConferenceBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers;

/// <summary>
/// Контролер надання аналітичних звітів з бронювання.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportApplicationService _reportApplicationService;

    /// <summary>
    /// Конструктор класу ініціалізує контролер звітів.
    /// </summary>
    /// <param name="reportApplicationService">
    /// Сервіс для формування звітів.
    /// </param>
    public ReportsController(
        IReportApplicationService reportApplicationService)
    {
        _reportApplicationService = reportApplicationService;
    }

    /// <summary>
    /// Метод повернення зведеної статистики з бронювання за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Зведена статистика з бронировань.</returns>
    [HttpGet("bookings")]
    [ProducesResponseType(typeof(BookingSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingSummaryResponse>> GetBookingSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _reportApplicationService
            .GetBookingSummaryAsync(from, to);

        return Ok(result);
    }

    /// <summary>
    /// Метод повертає статистику завантаження залів за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика використання залів.</returns>
    [HttpGet("halls")]
    [ProducesResponseType(typeof(IReadOnlyCollection<HallUtilizationResponse>),     StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyCollection<HallUtilizationResponse>>>
        GetHallUtilization([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _reportApplicationService
            .GetHallUtilizationAsync(from, to);

        return Ok(result);
    }

    /// <summary>
    /// Метод повернення статистики використання послуг за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика популярності послуг.</returns>
    [HttpGet("services")]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<PopularServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyCollection<PopularServiceResponse>>>
        GetPopularServices([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _reportApplicationService
            .GetPopularServicesAsync(from, to);

        return Ok(result);
    }
}