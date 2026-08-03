using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>
    /// Інтерфейс репозиторію для роботи з залами конференцій.
    /// </summary>
    public interface IHallRepository
    {
        /// <summary>
        /// Сигнатура методу для отримання залу за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Guid id залу</param>
        /// <returns></returns>
        Task<Hall?> GetByIdAsync(Guid id);

        /// <summary>
        /// Сигнатура методу для отримання всіх залів.
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<Hall>> GetAllAsync();

        /// <summary>
        /// Сигнатура методу для отримання доступних залів на певний період часу з урахуванням місткості.
        /// </summary>
        /// <param name="startTime">Початковий час періоду</param>
        /// <param name="endTime">Кінцевий час періоду</param>
        /// <param name="capacity">Мінімальна місткість залу</param>
        /// <returns>Список доступних залів</returns>
        Task<IReadOnlyList<Hall>> GetAvailableAsync(
            DateTime startTime,
            DateTime endTime,
            int capacity);

        /// <summary>
        /// Сигнатура методу для додавання нового залу.
        /// </summary>
        /// <param name="hall">Зал для додавання</param>
        /// <returns></returns>
        Task AddAsync(Hall hall);

        /// <summary>
        /// Сигнатура методу для оновлення інформації про зал.
        /// </summary>
        /// <param name="hall">Зал для оновлення</param>
        void Update(Hall hall);

        /// <summary>
        /// Сигнатура методу для видалення залу.
        /// </summary>
        /// <param name="hall">Зал для видалення</param>
        void Delete(Hall hall);

        /// <summary>
        /// Сигнатура методу для перевірки існування залу за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Guid id залу</param>
        /// <returns>True, якщо зал існує, інакше False</returns>
        Task<bool> ExistsAsync(Guid id);
    }
}
