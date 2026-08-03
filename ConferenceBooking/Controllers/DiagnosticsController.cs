using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers;

/// <summary>
/// Клас контролера для тестування репозиторіїв. Забезпечує доступ до методів репозиторію для перевірки їх роботи.
/// </summary>
[ApiController]
[Route("api/diagnostics")]
public class DiagnosticsController : ControllerBase
{
    /// <summary>
    /// Інтерфейс репозиторію для роботи з залами конференцій. 
    /// </summary>
    private readonly IHallRepository _hallRepository;

    /// <summary>
    /// Інтерфейс репозиторію для роботи з послугами конференцій.
    /// </summary>
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Інтерфейс репозиторію для роботи з бронюваннями конференцій.
    /// </summary>
    private readonly IBookingRepository _bookingRepository;

    /// <summary>
    /// Конструктор класу RepositoryTestController, який приймає інтерфейс репозиторію для роботи з залами конференцій як параметр.
    /// </summary>
    /// <param name="hallRepository">Інтерфейс репозиторію для роботи з залами конференцій.</param>
    public DiagnosticsController(IHallRepository hallRepository, IServiceRepository serviceRepository, IBookingRepository bookingRepository)
    {
        _hallRepository = hallRepository;
        _serviceRepository = serviceRepository;
        _bookingRepository = bookingRepository;
    }

    /// <summary>
    /// Get-запит для отримання всіх залів конференцій. Повертає список всіх залів у форматі JSON.
    /// </summary>
    /// <returns></returns>
    [HttpGet("halls")]
    public async Task<IActionResult> GetHalls()
    {
        var halls = await _hallRepository.GetAllAsync();

        return Ok(halls);
    }

    /// <summary>
    /// Get-запит для отримання доступних залів конференцій на основі заданого часу та місткості. Повертає список доступних залів у форматі JSON.
    /// </summary>
    /// <param name="startTime">Час початку конференції.</param>
    /// <param name="endTime">Час завершення конференції.</param>
    /// <param name="capacity">Місткість залу.</param>
    /// <returns>Список доступних залів конференцій у форматі JSON.</returns>
    [HttpGet("halls/available")]
    public async Task<IActionResult> GetAvailableHalls(
    DateTime startTime,
    DateTime endTime,
    int capacity)
    {
        var halls = await _hallRepository.GetAvailableAsync(
            startTime,
            endTime,
            capacity);

        return Ok(halls);
    }

    /// <summary>
    /// Get-запит для отримання всіх послуг конференцій. Повертає список всіх послуг у форматі JSON.
    /// </summary>
    /// <returns></returns>
    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var services = await _serviceRepository.GetAllAsync();

        return Ok(services);
    }

    /// <summary>
    /// Get-запит для отримання конкретної послуги конференції за її ідентифікатором. Повертає об'єкт послуги у форматі JSON або статус 404, якщо послуга не знайдена.
    /// </summary>
    /// <param name="id">Ідентифікатор послуги конференції.</param>
    /// <returns></returns>
    [HttpGet("services/{id:guid}")]
    public async Task<IActionResult> GetService(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);

        if (service is null)
            return NotFound();

        return Ok(service);
    }

    /// <summary>
    /// Get-запит для перевірки існування конкретної послуги конференції за її ідентифікатором. Повертає true, якщо послуга існує, або false, якщо не існує.
    /// </summary>
    /// <param name="id">Ідентифікатор послуги конференції.</param>
    /// <returns></returns>
    [HttpGet("services/exists/{id:guid}")]
    public async Task<IActionResult> ServiceExists(Guid id)
    {
        var exists = await _serviceRepository.ExistsAsync(id);

        return Ok(exists);
    }

    /// <summary>
    /// Get-запит для отримання всіх бронювань конкретного залу конференції за його ідентифікатором. Повертає список бронювань у форматі JSON.
    /// </summary>
    /// <param name="hallId">Ідентифікатор залу конференції.</param>
    /// <returns></returns>
    [HttpGet("bookings/hall/{hallId:guid}")]
    public async Task<IActionResult> GetBookingsByHall(Guid hallId)
    {
        var bookings = await _bookingRepository.GetByHallAsync(hallId);

        return Ok(bookings);
    }

    /// <summary>
    /// Get-запит для перевірки наявності конфліктів бронювання для конкретного залу конференції за його ідентифікатором та заданим періодом часу. Повертає true, якщо конфлікт існує, або false, якщо не існує.
    /// </summary>
    /// <param name="hallId">Ідентифікатор залу конференції.</param>
    /// <param name="startTime">Час початку конференції.</param>
    /// <param name="endTime">Час завершення конференції.</param>
    /// <returns></returns>
    [HttpGet("bookings/conflict")]
    public async Task<IActionResult> HasBookingConflict(Guid hallId, DateTime startTime, DateTime endTime)
    {
        var hasConflict = await _bookingRepository.HasConflictAsync(
            hallId,
            startTime,
            endTime);

        return Ok(new
        {
            hallId,
            startTime,
            endTime,
            hasConflict
        });
    }
}