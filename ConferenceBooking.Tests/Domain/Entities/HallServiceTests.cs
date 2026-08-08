using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Tests.Domain.Entities
{
    public class HallServiceTests
    {
        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта HallService
        /// з валідними ідентифікаторами об'єкт створюється успішно.
        /// </summary>
        [Fact]
        public void Create_WithValidIds_ShouldCreateHallService()
        {
            // Arrange
            var hallId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();

            // Act
            var hallService = HallService.Create(
                hallId,
                serviceId);

            // Assert
            Assert.Equal(hallId, hallService.HallId);
            Assert.Equal(serviceId, hallService.ServiceId);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта HallService
        /// з порожнім HallId викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEmptyHallId_ShouldThrowDomainException()
        {
            // Arrange
            var serviceId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                HallService.Create(
                    Guid.Empty,
                    serviceId));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта HallService
        /// з порожнім ServiceId викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEmptyServiceId_ShouldThrowDomainException()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                HallService.Create(
                    hallId,
                    Guid.Empty));
        }
    }
}