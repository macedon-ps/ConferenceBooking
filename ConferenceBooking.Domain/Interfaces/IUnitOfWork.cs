namespace ConferenceBooking.Domain.Interfaces;

/// <summary>
/// Інтерфейс для реалізації патерну "Unit of Work". Містить метод для збереження змін у базі даних.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сигнатура методу для збереження змін у базі даних. Повертає кількість змінених записів у базі даних після виконання операцій збереження.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();
}
