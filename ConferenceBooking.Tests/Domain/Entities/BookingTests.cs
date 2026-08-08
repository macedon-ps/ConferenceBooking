using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Tests.Domain.Entities
{
    public class BookingTests
    {
        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Booking з валідними даними
        /// об'єкт створюється успішно.
        /// </summary>
        [Fact]
        public void Create_WithValidData_ShouldCreateBooking()
        {
            // Arrange
            var hallId = Guid.NewGuid();
            var startTime = new DateTime(2026, 8, 10, 10, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 14, 0, 0);

            // Act
            var booking = Booking.Create(
                hallId,
                startTime,
                endTime);

            // Assert
            Assert.NotEqual(Guid.Empty, booking.Id);
            Assert.Equal(hallId, booking.HallId);
            Assert.Equal(startTime, booking.StartTime);
            Assert.Equal(endTime, booking.EndTime);
            Assert.Empty(booking.Services);
            Assert.Equal(0, booking.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Booking з порожнім HallId
        /// викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEmptyHallId_ShouldThrowDomainException()
        {
            // Act & Assert
            Assert.Throws<DomainException>(() =>
                Booking.Create(
                    Guid.Empty,
                    new DateTime(2026, 8, 10, 10, 0, 0),
                    new DateTime(2026, 8, 10, 14, 0, 0)));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Booking,
        /// якщо StartTime пізніше EndTime, викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithStartTimeAfterEndTime_ShouldThrowDomainException()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                Booking.Create(
                    hallId,
                    new DateTime(2026, 8, 10, 14, 0, 0),
                    new DateTime(2026, 8, 10, 10, 0, 0)));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Booking
        /// з однаковими StartTime та EndTime викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEqualStartAndEndTime_ShouldThrowDomainException()
        {
            // Arrange
            var hallId = Guid.NewGuid();
            var time = new DateTime(2026, 8, 10, 10, 0, 0);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                Booking.Create(
                    hallId,
                    time,
                    time));
        }

        /// <summary>
        /// Метод перевіряє, що при додаванні валідної послуги
        /// послуга успішно додається до бронювання.
        /// </summary>
        [Fact]
        public void AddService_WithValidId_ShouldAddService()
        {
            // Arrange
            var booking = Booking.Create(
                Guid.NewGuid(),
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 14, 0, 0));

            var serviceId = Guid.NewGuid();

            // Act
            booking.AddService(serviceId);

            // Assert
            Assert.Single(booking.Services);
            Assert.Equal(serviceId, booking.Services.First().ServiceId);
        }

        /// <summary>
        /// Метод перевіряє, що при додаванні послуги з порожнім ServiceId
        /// викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void AddService_WithEmptyServiceId_ShouldThrowDomainException()
        {
            // Arrange
            var booking = Booking.Create(
                Guid.NewGuid(),
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 14, 0, 0));

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                booking.AddService(Guid.Empty));
        }

        /// <summary>
        /// Метод перевіряє, що повторне додавання тієї самої послуги
        /// викидає виключення DomainException.
        /// </summary>
        [Fact]
        public void AddService_WithDuplicateId_ShouldThrowDomainException()
        {
            // Arrange
            var booking = Booking.Create(
                Guid.NewGuid(),
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 14, 0, 0));

            var serviceId = Guid.NewGuid();

            booking.AddService(serviceId);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                booking.AddService(serviceId));
        }

        /// <summary>
        /// Метод перевіряє, що при видаленні існуючої послуги
        /// послуга успішно видаляється з бронювання.
        /// </summary>
        [Fact]
        public void RemoveService_WithExistingId_ShouldRemoveService()
        {
            // Arrange
            var booking = Booking.Create(
                Guid.NewGuid(),
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 14, 0, 0));

            var serviceId = Guid.NewGuid();

            booking.AddService(serviceId);

            // Act
            booking.RemoveService(serviceId);

            // Assert
            Assert.Empty(booking.Services);
        }

        /// <summary>
        /// Метод перевіряє, що при видаленні послуги, яка відсутня в бронюванні,
        /// викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void RemoveService_WithMissingId_ShouldThrowDomainException()
        {
            // Arrange
            var booking = Booking.Create(
                Guid.NewGuid(),
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 14, 0, 0));

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                booking.RemoveService(Guid.NewGuid()));
        }

        /// <summary>
        /// Метод перевіряє, що при встановленні валідної загальної вартості
        /// значення TotalCost успішно змінюється.
        /// </summary>
        [Fact]
        public void SetTotalCost_WithValidValue_ShouldSetTotalCost()
        {
            // Arrange
            var booking = Booking.Create(
                Guid.NewGuid(),
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 14, 0, 0));

            const decimal totalCost = 8500m;

            // Act
            booking.SetTotalCost(totalCost);

            // Assert
            Assert.Equal(totalCost, booking.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє, що при встановленні від'ємної загальної вартості
        /// викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void SetTotalCost_WithNegativeValue_ShouldThrowDomainException()
        {
            // Arrange
            var booking = Booking.Create(
                Guid.NewGuid(),
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 14, 0, 0));

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                booking.SetTotalCost(-100));
        }
    }
}