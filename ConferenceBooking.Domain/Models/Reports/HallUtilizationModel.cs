namespace ConferenceBooking.Domain.Models.Reports;

/// <summary>
/// Клас надає статистику використання конференц-залу.
/// </summary>
public class HallUtilizationModel
{
    /// <summary>
    /// Guid Id залу.
    /// </summary>
    public Guid HallId { get; init; }

    /// <summary>
    /// Назва залу.
    /// </summary>
    public string HallName { get; init; } = string.Empty;

    /// <summary>
    /// Кількість бронювань залу за вказаний період.
    /// </summary>
    public int BookingCount { get; init; }

    /// <summary>
    /// Загальна кількість заброньованих годин за вказаний період.
    /// </summary>
    public decimal TotalBookedHours { get; init; }

    /// <summary>
    /// Загальна вартість бронювань залу за вказаний період.
    /// </summary>
    public decimal TotalRevenue { get; init; }
}
