using ConferenceBooking.Application.DTOs.Services;

namespace ConferenceBooking.Application.DTOs.Halls;

/// <summary>
/// Клас відповіді для отримання інформації про зал. Містить унікальний ідентифікатор, назву, місткість, погодинну ставку та список послуг.
/// </summary>
public class HallResponse
{
    /// <summary>
    /// Guid Id залу. Містить унікальний ідентифікатор типу Guid, який використовується для ідентифікації конкретного залу.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Назва залу. Містить рядок, який представляє назву залу. За замовчуванням встановлено порожній рядок.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Місткість залу. Містить числове значення, яке вказує на максимальну кількість людей, які можуть перебувати у залі одночасно.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Погодинна ставка залу. Містить числове значення типу decimal, яке представляє вартість оренди залу за годину.
    /// </summary>
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// Список послуг, доступних у залі. Містить колекцію об'єктів типу ServiceResponse, які представляють різні послуги, що надаються разом із залом.
    /// </summary>
    public List<ServiceResponse> Services { get; set; } = new();
}