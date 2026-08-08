using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Models;

namespace ConferenceBooking.Application.Services
{
    public class BookingCostCalculator : IBookingCostCalculator
    {
        /// <summary>
        /// Клас для розрахунку вартості бронювання конференц-залу.
        /// </summary>
        /// <param name="startTime">Час початку бронювання</param>
        /// <param name="endTime">Час завершення бронювання</param>
        /// <param name="hourlyRate">Погодинна ставка за зал</param>
        /// <param name="servicesCost">Вартість додаткових послуг</param>
        /// <returns>Результат розрахунку вартості бронювання</returns>
        public BookingCostResult Calculate(DateTime startTime, DateTime endTime, decimal hourlyRate, decimal servicesCost)
        {
            decimal hallCost = 0m;
            var currentTime = startTime;

            while (currentTime < endTime)
            {
                var coefficient = GetTariffCoefficient(currentTime);

                var nextBoundary = GetNextTariffBoundary(currentTime);

                var segmentEnd = nextBoundary < endTime
                        ? nextBoundary
                        : endTime;

                var duration = (decimal)(segmentEnd - currentTime).TotalHours;

                hallCost += duration * hourlyRate * coefficient;

                currentTime = segmentEnd;
            }

            hallCost = decimal.Round(hallCost, 2, MidpointRounding.AwayFromZero);

            var totalCost = hallCost + servicesCost;

            return new BookingCostResult
            {
                HallCost = hallCost,
                TotalCost = totalCost
            };
        }

        /// <summary>
        /// Метод для отримання коефіцієнта тарифу на основі поточного часу.
        /// </summary>
        /// <param name="currentTime">Поточний час</param>
        /// <returns>Коефіцієнт тарифу</returns>
        private decimal GetTariffCoefficient(DateTime currentTime)
        {
            var time = currentTime.TimeOfDay;

            if (time >= TimeSpan.FromHours(6) &&
                time < TimeSpan.FromHours(9))
            {
                return 0.90m;
            }

            if (time >= TimeSpan.FromHours(9) &&
                time < TimeSpan.FromHours(12))
            {
                return 1.00m;
            }

            if (time >= TimeSpan.FromHours(12) &&
                time < TimeSpan.FromHours(14))
            {
                return 1.15m;
            }

            if (time >= TimeSpan.FromHours(14) &&
                time < TimeSpan.FromHours(18))
            {
                return 1.00m;
            }

            if (time >= TimeSpan.FromHours(18) &&
                time < TimeSpan.FromHours(23))
            {
                return 0.80m;
            }

            return 0.00m;
        }

        /// <summary>
        /// Метод для визначення наступної межі тарифу на основі поточного часу.
        /// </summary>
        /// <param name="currentTime">Поточний час</param>
        /// <returns>Час наступної межі тарифу</returns>
        private DateTime GetNextTariffBoundary(DateTime currentTime)
        {
            var date = currentTime.Date;
            var time = currentTime.TimeOfDay;

            if (time < TimeSpan.FromHours(6))
            {
                return date.AddHours(6);
            }

            if (time < TimeSpan.FromHours(9))
            {
                return date.AddHours(9);
            }

            if (time < TimeSpan.FromHours(12))
            {
                return date.AddHours(12);
            }

            if (time < TimeSpan.FromHours(14))
            {
                return date.AddHours(14);
            }

            if (time < TimeSpan.FromHours(18))
            {
                return date.AddHours(18);
            }

            if (time < TimeSpan.FromHours(23))
            {
                return date.AddHours(23);
            }

            return date.AddDays(1);
        }
    }
}
