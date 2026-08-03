using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>
    /// Інтерфейс репозиторію для роботи з послугами конференцій.
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Сигнатура методу для отримання послуги за її унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Guid id послуги</param>
        /// <returns>Послуга або null, якщо не знайдено</returns>
        Task<Service?> GetByIdAsync(Guid id);

        /// <summary>
        /// Сигнатура методу для отримання всіх послуг.
        /// </summary>
        /// <returns>Список всіх послуг</returns>
        Task<IReadOnlyList<Service>> GetAllAsync();

        /// <summary>
        /// Сигнатура методу для додавання нової послуги.
        /// </summary>
        /// <param name="service">Послуга для додавання</param>
        /// <returns></returns>     
        Task AddAsync(Service service);

        /// <summary>
        /// Сигнатура методу для оновлення існуючої послуги.
        /// </summary>
        /// <param name="service">Послуга для оновлення</param>
        void Update(Service service);

        /// <summary>
        /// Сигнатура методу для видалення послуги.
        /// </summary>
        /// <param name="service">Послуга для видалення</param>
        void Delete(Service service);

        /// <summary>
        /// Сигнатура методу для перевірки існування послуги за її унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Guid id послуги</param>
        /// <returns>True, якщо послуга існує, інакше False</returns>
        Task<bool> ExistsAsync(Guid id);
    }
}
