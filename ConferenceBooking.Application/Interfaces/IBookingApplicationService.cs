using ConferenceBooking.Application.DTOs.Bookings;

namespace ConferenceBooking.Application.Interfaces
{
    /// <summary>
    /// Інтерфейс сервісу для управління бронюваннями конференцій. Містить методи для створення бронювань.
    /// </summary>
    public interface IBookingApplicationService
    {
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
