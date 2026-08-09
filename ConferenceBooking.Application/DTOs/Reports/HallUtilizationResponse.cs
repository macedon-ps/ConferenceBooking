namespace ConferenceBooking.Application.DTOs.Reports;

/// <summary>
/// Клас надає статистику використання залів за вказаний період.
/// </summary>
public class HallUtilizationResponse
{
    /// <summary>
    /// Унікальний ідентифікатор конференц-залу.
    /// </summary>
    public Guid HallId { get; set; }

    /// <summary>
    /// Назва конференц-залу.
    /// </summary>
    public string HallName { get; set; } = string.Empty;

    /// <summary>
    /// Кількість бронювань залу.
    /// </summary>
    public int BookingCount { get; set; }

    /// <summary>
    /// Загальна кількість заброньованих годин.
    /// </summary>
    public decimal TotalBookedHours { get; set; }

    /// <summary>
    /// Загальна вартість бронювань залу.
    /// </summary>
    public decimal TotalRevenue { get; set; }
}
