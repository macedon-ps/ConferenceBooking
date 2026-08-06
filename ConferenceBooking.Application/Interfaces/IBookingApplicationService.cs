using ConferenceBooking.Application.DTOs.Bookings;

namespace ConferenceBooking.Application.Interfaces
{
    /// <summary>
    /// Інтерфейс сервісу для управління бронюваннями конференцій. Містить методи для створення бронювань.
    /// </summary>
    public interface IBookingApplicationService
    {
        /// <summary>
        /// Сигнатура методу для отримання всіх бронювань конференцій. Повертає колекцію об'єктів BookingResponse, які містять інформацію про всі існуючі бронювання.
        /// </summary>
        /// <returns>Колекція об'єктів BookingResponse з інформацією про всі існуючі бронювання.</returns>
        Task<IReadOnlyCollection<BookingResponse>> GetAllAsync();

        /// <summary>
        /// Сигнатура методу для отримання конкретного бронювання конференції за його унікальним ідентифікатором. Приймає Guid id бронювання та повертає об'єкт BookingResponse з інформацією про відповідне бронювання.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор бронювання.</param>
        /// <returns>Об'єкт BookingResponse з інформацією про відповідне бронювання.</returns>
        Task<BookingResponse> GetByIdAsync(Guid id);

        /// <summary>
        /// Сигнатура методу для отримання всіх бронювань конференцій за унікальним ідентифікатором залу. Приймає Guid hallId залу та повертає колекцію об'єктів BookingResponse з інформацією про відповідні бронювання.
        /// </summary>
        /// <param name="hallId">Унікальний ідентифікатор залу.</param>
        /// <returns>Колекція об'єктів BookingResponse з інформацією про відповідні бронювання.</returns>
        Task<IReadOnlyCollection<BookingResponse>> GetByHallAsync(Guid hallId);

        /// <summary>
        /// Сигнатура методу для створення нового бронювання конференції. Приймає об'єкт CreateBookingRequest, який містить необхідні дані для створення бронювання, та повертає об'єкт BookingResponse з інформацією про створене бронювання.
        /// </summary>
        /// <param name="request">Об'єкт запиту для створення бронювання.</param>
        /// <returns>Об'єкт відповіді з інформацією про створене бронювання.</returns>
        Task<BookingResponse> CreateAsync(CreateBookingRequest request);

        /// <summary>
        /// Сигнатура методу для видалення існуючого бронювання конференції за його унікальним ідентифікатором. Приймає Guid id бронювання, яке потрібно видалити.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор бронювання, яке потрібно видалити.</param>
        /// <returns>Завершення завдання без повернення значення.</returns>
        Task DeleteAsync(Guid id);
    }
}
