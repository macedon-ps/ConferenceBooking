namespace ConferenceBooking.Domain.Models.Reports;

/// <summary>
/// Клас надає зведену статистику по бронюванням.
/// </summary>
public class BookingSummaryModel
{
    /// <summary>
    /// Загальна кількість бронювань за вказаний період.
    /// </summary>
    public int TotalBookings { get; init; }

    /// <summary>
    /// Загальна кількість заброньованих годин за вказаний період.
    /// </summary>
    public decimal TotalBookedHours { get; init; }

    /// <summary>
    /// Загальна вартість всіх бронювань за вказаний період.
    /// </summary>
    public decimal TotalRevenue { get; init; }

    /// <summary>
    /// Середня вартість одного бронювання за вказаний період.
    /// </summary>
    public decimal AverageBookingCost { get; init; }
}
