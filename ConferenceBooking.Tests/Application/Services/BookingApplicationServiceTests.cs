using ConferenceBooking.Application.DTOs.Bookings;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Models;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConferenceBooking.Tests.Application.Services
{
    public class BookingApplicationServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IHallRepository> _hallRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<BookingApplicationService>> _loggerMock;
        private readonly Mock<IBookingCostCalculator> _costCalculatorMock;

        private readonly BookingApplicationService _service;

        public BookingApplicationServiceTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _hallRepositoryMock = new Mock<IHallRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<BookingApplicationService>>();
            _costCalculatorMock = new Mock<IBookingCostCalculator>();

            _service = new BookingApplicationService(
                _bookingRepositoryMock.Object,
                _hallRepositoryMock.Object,
                _serviceRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _costCalculatorMock.Object);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні бронювання
        /// з валідними даними бронювання успішно створюється.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateBooking()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            var service = Service.Create(
                "Projector",
                500);

            hall.AddService(service.Id);

            var request = new CreateBookingRequest
            {
                HallId = hall.Id,
                StartTime = new DateTime(2026, 8, 10, 10, 0, 0),
                EndTime = new DateTime(2026, 8, 10, 12, 0, 0),
                ServiceIds = [service.Id]
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(repository => repository.HasConflictAsync(
                    hall.Id,
                    request.StartTime,
                    request.EndTime))
                .ReturnsAsync(false);

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service> { service });

            _costCalculatorMock
                .Setup(calculator => calculator.Calculate(
                    request.StartTime,
                    request.EndTime,
                    hall.HourlyRate,
                    service.Price))
                .Returns(new BookingCostResult
                {
                    HallCost = 4000m,
                    TotalCost = 4500m
                });

            _bookingRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<Booking>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hall.Id, result.HallId);
            Assert.Equal(request.StartTime, result.StartTime);
            Assert.Equal(request.EndTime, result.EndTime);
            Assert.Equal(4500m, result.TotalCost);

            var resultService = Assert.Single(result.Services);

            Assert.Equal(service.Id, resultService.Id);
            Assert.Equal("Projector", resultService.Name);
            Assert.Equal(500m, resultService.Price);

            _bookingRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.Is<Booking>(booking =>
                        booking.HallId == hall.Id &&
                        booking.StartTime == request.StartTime &&
                        booking.EndTime == request.EndTime &&
                        booking.TotalCost == 4500m)),
                Times.Once);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Once);

            _costCalculatorMock.Verify(
                calculator => calculator.Calculate(
                    request.StartTime,
                    request.EndTime,
                    hall.HourlyRate,
                    500m),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні бронювання
        /// для неіснуючого залу викидається KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithMissingHall_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            var request = new CreateBookingRequest
            {
                HallId = hallId,
                StartTime = new DateTime(2026, 8, 10, 10, 0, 0),
                EndTime = new DateTime(2026, 8, 10, 12, 0, 0),
                ServiceIds = []
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hallId))
                .ReturnsAsync((Hall?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.CreateAsync(request));

            _bookingRepositoryMock.Verify(
                repository => repository.HasConflictAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()),
                Times.Never);

            _bookingRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.IsAny<Booking>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що при наявності конфлікту часу
        /// викидається InvalidOperationException.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithBookingConflict_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            var request = new CreateBookingRequest
            {
                HallId = hall.Id,
                StartTime = new DateTime(2026, 8, 10, 10, 0, 0),
                EndTime = new DateTime(2026, 8, 10, 12, 0, 0),
                ServiceIds = []
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(repository => repository.HasConflictAsync(
                    hall.Id,
                    request.StartTime,
                    request.EndTime))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(request));

            _serviceRepositoryMock.Verify(
                repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()),
                Times.Never);

            _bookingRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.IsAny<Booking>()),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що при вказанні неіснуючої послуги
        /// викидається KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithMissingService_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            var missingServiceId = Guid.NewGuid();

            var request = new CreateBookingRequest
            {
                HallId = hall.Id,
                StartTime = new DateTime(2026, 8, 10, 10, 0, 0),
                EndTime = new DateTime(2026, 8, 10, 12, 0, 0),
                ServiceIds = [missingServiceId]
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(repository => repository.HasConflictAsync(
                    hall.Id,
                    request.StartTime,
                    request.EndTime))
                .ReturnsAsync(false);

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service>());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.CreateAsync(request));

            _costCalculatorMock.Verify(
                calculator => calculator.Calculate(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<decimal>(),
                    It.IsAny<decimal>()),
                Times.Never);

            _bookingRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.IsAny<Booking>()),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що послуга, яка існує в системі,
        /// але не доступна у вибраному залі, не може бути заброньована.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithUnavailableService_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            var service = Service.Create(
                "Projector",
                500);

            var request = new CreateBookingRequest
            {
                HallId = hall.Id,
                StartTime = new DateTime(2026, 8, 10, 10, 0, 0),
                EndTime = new DateTime(2026, 8, 10, 12, 0, 0),
                ServiceIds = [service.Id]
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(repository => repository.HasConflictAsync(
                    hall.Id,
                    request.StartTime,
                    request.EndTime))
                .ReturnsAsync(false);

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service> { service });

            // Service существует, но hall.AddService(service.Id) не вызывался.

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(request));

            _costCalculatorMock.Verify(
                calculator => calculator.Calculate(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<decimal>(),
                    It.IsAny<decimal>()),
                Times.Never);

            _bookingRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.IsAny<Booking>()),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що дублікати ServiceIds
        /// видаляються перед перевіркою та створенням бронювання.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithDuplicateServiceIds_ShouldProcessEachServiceOnce()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            var service = Service.Create(
                "Projector",
                500);

            hall.AddService(service.Id);

            var request = new CreateBookingRequest
            {
                HallId = hall.Id,
                StartTime = new DateTime(2026, 8, 10, 10, 0, 0),
                EndTime = new DateTime(2026, 8, 10, 12, 0, 0),
                ServiceIds = [service.Id, service.Id]
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(repository => repository.HasConflictAsync(
                    hall.Id,
                    request.StartTime,
                    request.EndTime))
                .ReturnsAsync(false);

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.Is<IEnumerable<Guid>>(ids =>
                        ids.Count() == 1 &&
                        ids.Single() == service.Id)))
                .ReturnsAsync(new List<Service> { service });

            _costCalculatorMock
                .Setup(calculator => calculator.Calculate(
                    request.StartTime,
                    request.EndTime,
                    hall.HourlyRate,
                    service.Price))
                .Returns(new BookingCostResult
                {
                    HallCost = 4000m,
                    TotalCost = 4500m
                });

            _bookingRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<Booking>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            var resultService = Assert.Single(result.Services);

            Assert.Equal(service.Id, resultService.Id);
            Assert.Equal(4500m, result.TotalCost);

            _serviceRepositoryMock.Verify(
                repository => repository.GetByIdsAsync(
                    It.Is<IEnumerable<Guid>>(ids =>
                        ids.Count() == 1 &&
                        ids.Single() == service.Id)),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що GetByIdAsync повертає
        /// правильно перетворене бронювання.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_WithExistingBooking_ShouldReturnMappedBooking()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            var service = Service.Create(
                "Projector",
                500);

            var booking = Booking.Create(
                hallId,
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 12, 0, 0));

            booking.AddService(service.Id);
            booking.SetTotalCost(4500m);

            _bookingRepositoryMock
                .Setup(repository => repository.GetByIdAsync(booking.Id))
                .ReturnsAsync(booking);

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service> { service });

            // Act
            var result = await _service.GetByIdAsync(booking.Id);

            // Assert
            Assert.Equal(booking.Id, result.Id);
            Assert.Equal(hallId, result.HallId);
            Assert.Equal(booking.StartTime, result.StartTime);
            Assert.Equal(booking.EndTime, result.EndTime);
            Assert.Equal(4500m, result.TotalCost);

            var resultService = Assert.Single(result.Services);

            Assert.Equal(service.Id, resultService.Id);
            Assert.Equal("Projector", resultService.Name);
            Assert.Equal(500m, resultService.Price);
        }

        /// <summary>
        /// Метод перевіряє, що GetByIdAsync для неіснуючого бронювання
        /// викидає KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_WithMissingBooking_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var bookingId = Guid.NewGuid();

            _bookingRepositoryMock
                .Setup(repository => repository.GetByIdAsync(bookingId))
                .ReturnsAsync((Booking?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetByIdAsync(bookingId));

            _serviceRepositoryMock.Verify(
                repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що GetByHallAsync для неіснуючого залу
        /// викидає KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task GetByHallAsync_WithMissingHall_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hallId))
                .ReturnsAsync((Hall?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetByHallAsync(hallId));

            _bookingRepositoryMock.Verify(
                repository => repository.GetByHallAsync(
                    It.IsAny<Guid>()),
                Times.Never);

            _serviceRepositoryMock.Verify(
                repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що DeleteAsync успішно видаляє
        /// існуюче бронювання та зберігає зміни.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_WithExistingBooking_ShouldDeleteBooking()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            var booking = Booking.Create(
                hallId,
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 12, 0, 0));

            _bookingRepositoryMock
                .Setup(repository => repository.GetByIdAsync(booking.Id))
                .ReturnsAsync(booking);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            await _service.DeleteAsync(booking.Id);

            // Assert
            _bookingRepositoryMock.Verify(
                repository => repository.Delete(booking),
                Times.Once);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що DeleteAsync для неіснуючого бронювання
        /// викидає KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_WithMissingBooking_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var bookingId = Guid.NewGuid();

            _bookingRepositoryMock
                .Setup(repository => repository.GetByIdAsync(bookingId))
                .ReturnsAsync((Booking?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.DeleteAsync(bookingId));

            _bookingRepositoryMock.Verify(
                repository => repository.Delete(
                    It.IsAny<Booking>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Never);
        }
    }
}