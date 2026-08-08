using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Tests.Domain.Entities
{
    public class BookingServiceTests
    {
        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта BookingService
        /// з валідними ідентифікаторами об'єкт створюється успішно.
        /// </summary>
        [Fact]
        public void Create_WithValidIds_ShouldCreateBookingService()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();

            // Act
            var bookingService = BookingService.Create(
                bookingId,
                serviceId);

            // Assert
            Assert.Equal(bookingId, bookingService.BookingId);
            Assert.Equal(serviceId, bookingService.ServiceId);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта BookingService
        /// з порожнім BookingId викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEmptyBookingId_ShouldThrowDomainException()
        {
            // Arrange
            var serviceId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                BookingService.Create(
                    Guid.Empty,
                    serviceId));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта BookingService
        /// з порожнім ServiceId викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEmptyServiceId_ShouldThrowDomainException()
        {
            // Arrange
            var bookingId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                BookingService.Create(
                    bookingId,
                    Guid.Empty));
        }
    }
}