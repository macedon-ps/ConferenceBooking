using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Клас BookingService представляє замовлену послуги. Він містить інформацію про послугу, її ціну та пов'язане бронювання.
    /// </summary>
    public class BookingService
    {
        /// <summary>
        /// Guid Id бронювання, до якого належить замовлена послуга. Зовнішній ключ для зв'язку з таблицею Booking.
        /// </summary>
        public Guid BookingId { get; private set; }

        /// <summary>
        /// Guid Id послуги, яка була замовлена. Зовнішній ключ для зв'язку з таблицею Service.
        /// </summary>
        public Guid ServiceId { get; private set; }

        private BookingService(Guid bookingId, Guid serviceId)
        {
            BookingId = bookingId;
            ServiceId = serviceId;
        }

        public static BookingService Create(Guid bookingId, Guid serviceId)
        {
            if (bookingId == Guid.Empty)
                throw new DomainException(
                    "Booking ID cannot be empty.");

            if (serviceId == Guid.Empty)
                throw new DomainException(
                    "Service ID cannot be empty.");

            return new BookingService(
                bookingId,
                serviceId);
        }
    }
}
