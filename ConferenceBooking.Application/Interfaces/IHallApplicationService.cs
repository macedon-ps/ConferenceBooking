using ConferenceBooking.Application.DTOs.Halls;

namespace ConferenceBooking.Application.Interfaces;

/// <summary>
/// Інтерфейс сервісу для управління залами конференцій. Містить методи для створення, оновлення, видалення та отримання доступних залів.
/// </summary>
public interface IHallApplicationService
{
    /// <summary>
    /// Сигнатура методу для створення нового залу конференцій.
    /// </summary>
    /// <param name="request">Об'єкт запиту для створення залу.</param>
    /// <returns>Об'єкт відповіді з інформацією про створений зал.</returns>    
    Task<HallResponse> CreateAsync(CreateHallRequest request);

    /// <summary>
    /// Сигнатура методу для оновлення існуючого залу конференцій.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу, який потрібно оновити.</param>
    /// <param name="request">Об'єкт запиту для оновлення залу.</param>
    /// <returns>Об'єкт відповіді з інформацією про оновлений зал.</returns>
    Task<HallResponse> UpdateAsync(
        Guid id,
        UpdateHallRequest request);
    
    /// <summary>
    /// Сигнатура методу для видалення існуючого залу конференцій.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу, який потрібно видалити.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Сигнатура методу для отримання списку доступних залів конференцій на основі заданих критеріїв.
    /// </summary>
    /// <param name="request">Об'єкт запиту для отримання доступних залів.</param>
    /// <returns>Колекція об'єктів відповіді з інформацією про доступні зали.</returns>
    Task<IReadOnlyCollection<HallResponse>> GetAvailableAsync(AvailableHallsRequest request);
}
