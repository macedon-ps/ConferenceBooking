namespace ConferenceBooking.Domain.Models.Reports;

/// <summary>
/// Клас надає статистику використання послуги.
/// </summary>
public class PopularServiceModel
{
    /// <summary>
    /// Guid Id послуги.
    /// </summary>
    public Guid ServiceId { get; init; }

    /// <summary>
    /// Назва послуги.
    /// </summary>
    public string ServiceName { get; init; } = string.Empty;

    /// <summary>
    /// Кількість використань послуги в бронюваннях за вказаний період.
    /// </summary>
    public int UsageCount { get; init; }
}
