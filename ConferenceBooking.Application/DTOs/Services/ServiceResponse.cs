namespace ConferenceBooking.Application.DTOs.Services;

/// <summary>
/// Клас відповіді для отримання інформації про послугу. Містить унікальний ідентифікатор, назву та ціну послуги.
/// </summary>
public class ServiceResponse
{
    /// <summary>
    /// Guid Id послуги. Містить унікальний ідентифікатор типу Guid, який використовується для ідентифікації конкретної послуги.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Назва послуги. Містить рядок, який представляє назву послуги. За замовчуванням встановлено порожній рядок.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ціна послуги. Містить числове значення типу decimal, яке представляє вартість послуги.
    /// </summary>
    public decimal Price { get; set; }
}
