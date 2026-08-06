using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>
    /// Інтерфейс репозиторію для роботи з бронюваннями конференцій.
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Сигнатура методу для отримання всіх бронювань.
        /// </summary>
        /// <returns>Список всіх бронювань</returns>
        Task<IReadOnlyList<Booking>> GetAllAsync();

        /// <summary>
        /// Сигнатура методу для отримання бронювання за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Guid id бронювання</param>
        /// <returns>Бронювання або null, якщо не знайдено</returns>
        Task<Booking?> GetByIdAsync(Guid id);

        /// <summary>
        /// Сигнатура методу для отримання всіх бронювань для певного залу.
        /// </summary>
        /// <param name="hallId">Guid id залу</param>
        /// <returns>Список бронювань</returns>
        Task<IReadOnlyList<Booking>> GetByHallAsync(Guid hallId);

        /// <summary>
        /// Сигнатура методу для перевірки наявності конфліктів бронювання для певного залу в заданий проміжок часу.
        /// </summary>
        /// <param name="hallId">Guid id залу</param>
        /// <param name="startTime">Час початку бронювання</param>
        /// <param name="endTime">Час завершення бронювання</param>
        /// <returns>True, якщо є конфлікт, інакше False</returns>
        Task<bool> HasConflictAsync(
            Guid hallId,
            DateTime startTime,
            DateTime endTime);

        /// <summary>
        /// Сигнатура методу для додавання нового бронювання.
        /// </summary>
        /// <param name="booking">Бронювання для додавання</param>
        /// <returns></returns>
        Task AddAsync(Booking booking);

        /// <summary>
        /// Сигнатура методу для оновлення існуючого бронювання.
        /// </summary>
        /// <param name="booking">Бронювання для оновлення</param>
        void Update(Booking booking);

        /// <summary>
        /// Сигнатура методу для видалення існуючого бронювання.
        /// </summary>
        /// <param name="booking">Бронювання для видалення</param>
        void Delete(Booking booking);
    }
}
