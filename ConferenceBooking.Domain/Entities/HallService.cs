using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Клас HallService представляє зв'язок між конференц-залом та послугою, яку можна замовити разом із бронюванням цього залу. Він містить інформацію про ідентифікатори конференц-залу та послуги.
    /// </summary>
    public class HallService
    {
        /// <summary>
        /// Guid Id конференц-залу.
        /// </summary>
        public Guid HallId { get; private set; }

        /// <summary>
        /// Guid Id послуги.
        /// </summary>
        public Guid ServiceId { get; private set; }

        private HallService(Guid hallId, Guid serviceId)
        {
            HallId = hallId;
            ServiceId = serviceId;
        }

        /// <summary>
        /// Метод Create створює новий екземпляр класу HallService, перевіряючи, чи не є передані ідентифікатори конференц-залу та послуги порожніми. Якщо будь-який з ідентифікаторів є порожнім, метод викидає DomainException.
        /// </summary>
        /// <param name="hallId">Ідентифікатор конференц-залу.</param>
        /// <param name="serviceId">Ідентифікатор послуги.</param>
        /// <returns>Новий екземпляр класу HallService.</returns>
        /// <exception cref="DomainException"></exception>
        public static HallService Create(Guid hallId, Guid serviceId)
        {
            if (hallId == Guid.Empty)
                throw new DomainException(
                    "Hall ID cannot be empty.");

            if (serviceId == Guid.Empty)
                throw new DomainException(
                    "Service ID cannot be empty.");

            return new HallService(
                hallId,
                serviceId);
        }
    }
}
