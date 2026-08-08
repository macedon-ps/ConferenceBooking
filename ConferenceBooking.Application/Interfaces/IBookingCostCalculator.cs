using ConferenceBooking.Application.Models;

namespace ConferenceBooking.Application.Interfaces
{
    /// <summary>
    /// Інтерфейс для розрахунку вартості бронювання залу.
    /// </summary>
    public interface IBookingCostCalculator
    {
        /// <summary>
        /// Сигнатура методу для розрахунку вартості бронювання залу.
        /// </summary>
        /// <param name="startTime">Час початку бронювання</param>
        /// <param name="endTime">Час завершення бронювання</param>
        /// <param name="hourlyRate">Погодинна ставка за зал</param>
        /// <param name="servicesCost">Вартість додаткових послуг</param>
        /// <returns>Результат розрахунку вартості бронювання</returns>
        BookingCostResult Calculate(DateTime startTime, DateTime endTime, decimal hourlyRate, decimal servicesCost);
    }
}
