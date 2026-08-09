namespace ConferenceBooking.Application.DTOs.Reports;

/// <summary>
/// Клас надає статистику використання послуг за вказаний період.
/// </summary>
public class PopularServiceResponse
{
    /// <summary>
    /// Унікальний ідентифікатор послуги.
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Назва послуги.
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Кількість використань послуги в бронюваннях.
    /// </summary>
    public int UsageCount { get; set; }
}