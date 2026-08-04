using ConferenceBooking.Application.DTOs.Services;

namespace ConferenceBooking.Application.DTOs.Bookings;

/// <summary>
/// Клас відповіді для отримання інформації про бронювання. Містить унікальний ідентифікатор бронювання, ідентифікатор залу, початковий та кінцевий час бронювання, список послуг та загальну вартість бронювання.
/// </summary>
public class BookingResponse
{
    /// <summary>
    /// Guid Id бронювання. Містить унікальний ідентифікатор типу Guid, який використовується для ідентифікації конкретного бронювання.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Guid Id залу. Містить унікальний ідентифікатор типу Guid, який використовується для ідентифікації конкретного залу, який було заброньовано.
    /// </summary>
    public Guid HallId { get; set; }

    /// <summary>
    /// Початковий час бронювання. Містить значення типу DateTime, яке вказує на початок періоду, протягом якого було заброньовано зал.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Кінцевий час бронювання. Містить значення типу DateTime, яке вказує на кінець періоду, протягом якого було заброньовано зал.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Список послуг, доданих до бронювання. Містить колекцію об'єктів типу ServiceResponse, які представляють різні послуги, що були додані до бронювання.
    /// </summary>
    public List<ServiceResponse> Services { get; set; } = new();

    /// <summary>
    /// Загальна вартість бронювання. Містить числове значення типу decimal, яке представляє сумарну вартість бронювання, включаючи вартість оренди залу та доданих послуг.
    /// </summary>
    public decimal TotalCost { get; set; }
}
