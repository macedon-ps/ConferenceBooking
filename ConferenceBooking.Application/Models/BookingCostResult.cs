namespace ConferenceBooking.Application.Models
{
    /// <summary>
    /// Клас, що представляє результат розрахунку вартості бронювання конференц-залу.
    /// </summary>
    public class BookingCostResult
    {
        /// <summary>
        /// Базова вартість бронювання залу.
        /// </summary>
        public decimal HallCost { get; init; }

        /// <summary>
        /// Повна вартість бронювання, включаючи базову вартість, тривалість бронювання та додаткові послуги.
        /// </summary>
        public decimal TotalCost { get; init; }
    }
}
