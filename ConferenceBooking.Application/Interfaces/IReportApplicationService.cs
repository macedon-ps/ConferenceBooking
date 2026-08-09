using ConferenceBooking.Application.DTOs.Reports;

namespace ConferenceBooking.Application.Interfaces;

/// <summary>
/// Інтерфейс сервісу для формування звітів та аналітики по бронюванням.
/// </summary>
public interface IReportApplicationService
{
    /// <summary>
    /// Повертає зведену статистику по бронюванням за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Зведена статистика по бронюванням.</returns>
    Task<BookingSummaryResponse> GetBookingSummaryAsync(DateTime from, DateTime to); 

    /// <summary>
    /// Повертає статистику використання конференц-залів за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика використання кожного залу.</returns>
    Task<IReadOnlyCollection<HallUtilizationResponse>> GetHallUtilizationAsync(DateTime from, DateTime to);

    /// <summary>
    /// Повертає статистику використання послуг за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика популярності послуг.</returns>
    Task<IReadOnlyCollection<PopularServiceResponse>> GetPopularServicesAsync(DateTime from, DateTime to);
}
