using ConferenceBooking.Domain.Models.Reports;

namespace ConferenceBooking.Domain.Interfaces;

/// <summary>
/// Інтерфейс для отримання аналітичних даних по бронюванням.
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Повертає зведену статистику по бронюванням за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Зведена статистика по бронюванням.</returns>
    Task<BookingSummaryModel> GetBookingSummaryAsync(DateTime from, DateTime to);

    /// <summary>
    /// Повертає статистику використання конференц-залів за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика використання конференц-залів.</returns>
    Task<IReadOnlyCollection<HallUtilizationModel>> GetHallUtilizationAsync(DateTime from, DateTime to);

    /// <summary>
    /// Повертає статистику використання послуг за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика використання послуг.</returns>
    Task<IReadOnlyCollection<PopularServiceModel>> GetPopularServicesAsync(DateTime from, DateTime to);
}