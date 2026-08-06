using ConferenceBooking.Application.DTOs.Services;

namespace ConferenceBooking.Application.Interfaces;

/// <summary>
/// Інтерфейс сервісу для управління послугами конференцій. Містить методи для отримання всіх послуг та отримання конкретної послуги за її унікальним ідентифікатором.
/// </summary>
public interface IServiceApplicationService
{
    /// <summary>
    /// Сигнатура методу для отримання всіх доступних послуг конференцій. Повертає колекцію об'єктів ServiceResponse, що містять інформацію про кожну послугу.
    /// </summary>
    /// <returns>Колекція об'єктів ServiceResponse з інформацією про всі послуги.</returns>
    Task<IReadOnlyCollection<ServiceResponse>> GetAllAsync();

    /// <summary>
    /// Сигнатура методу для отримання конкретної послуги конференцій за її унікальним ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор послуги.</param>
    /// <returns>Об'єкт ServiceResponse з інформацією про послугу.</returns>
    Task<ServiceResponse> GetByIdAsync(Guid id);
}