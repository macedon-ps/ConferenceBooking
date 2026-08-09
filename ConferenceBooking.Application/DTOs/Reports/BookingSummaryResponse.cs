namespace ConferenceBooking.Application.DTOs.Reports;

/// <summary>
/// Клас надає зведену статистику по бронюванням за вказаний період.
/// </summary>
public class BookingSummaryResponse
{
    /// <summary>
    /// Загальна кількість бронювань.
    /// </summary>
    public int TotalBookings { get; set; }

    /// <summary>
    /// Загальна кількість заброньованих годин.
    /// </summary>
    public decimal TotalBookedHours { get; set; }

    /// <summary>
    /// Загальна вартість всіх бронювань.
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Середня вартість одного бронювання.
    /// </summary>
    public decimal AverageBookingCost { get; set; }
}