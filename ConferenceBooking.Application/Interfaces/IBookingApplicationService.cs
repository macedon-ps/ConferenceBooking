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
    }
}
