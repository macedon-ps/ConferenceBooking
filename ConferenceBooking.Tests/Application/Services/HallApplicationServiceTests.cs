using ConferenceBooking.Application.DTOs.Halls;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConferenceBooking.Tests.Application.Services
{
    public class HallApplicationServiceTests
    {
        private readonly Mock<IHallRepository> _hallRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<HallApplicationService>> _loggerMock;

        private readonly HallApplicationService _service;

        public HallApplicationServiceTests()
        {
            _hallRepositoryMock = new Mock<IHallRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<HallApplicationService>>();

            _service = new HallApplicationService(
                _hallRepositoryMock.Object,
                _serviceRepositoryMock.Object,
                _bookingRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні залу з валідними даними
        /// зал успішно створюється та зберігається.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateHall()
        {
            // Arrange
            var serviceId = Guid.NewGuid();

            var service = Service.Create(
                "Projector",
                500);

            // Важно: используем реальный Id созданной услуги.
            serviceId = service.Id;

            var request = new CreateHallRequest
            {
                Name = "Conference Hall",
                Capacity = 100,
                HourlyRate = 2000,
                ServiceIds = [serviceId]
            };

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service> { service });

            _hallRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<Hall>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Conference Hall", result.Name);
            Assert.Equal(100, result.Capacity);
            Assert.Equal(2000, result.HourlyRate);

            Assert.Single(result.Services);
            Assert.Equal(serviceId, result.Services[0].Id);
            Assert.Equal("Projector", result.Services[0].Name);
            Assert.Equal(500, result.Services[0].Price);

            _hallRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.Is<Hall>(hall =>
                        hall.Name == "Conference Hall" &&
                        hall.Capacity == 100 &&
                        hall.HourlyRate == 2000)),
                Times.Once);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні залу з послугою,
        /// якої не існує, викидається KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithMissingService_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var requestedServiceId = Guid.NewGuid();

            var request = new CreateHallRequest
            {
                Name = "Conference Hall",
                Capacity = 100,
                HourlyRate = 2000,
                ServiceIds = [requestedServiceId]
            };

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service>());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.CreateAsync(request));

            _hallRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.IsAny<Hall>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що при оновленні існуючого залу
        /// з валідними даними зал успішно оновлюється.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateHall()
        {
            // Arrange
            var hall = Hall.Create(
                "Old Hall",
                50,
                1500);

            var service = Service.Create(
                "Projector",
                500);

            var request = new UpdateHallRequest
            {
                Name = "Updated Hall",
                Capacity = 100,
                HourlyRate = 2500,
                ServiceIds = [service.Id]
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service> { service });

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.UpdateAsync(
                hall.Id,
                request);

            // Assert
            Assert.Equal(hall.Id, result.Id);
            Assert.Equal("Updated Hall", result.Name);
            Assert.Equal(100, result.Capacity);
            Assert.Equal(2500, result.HourlyRate);

            Assert.Single(result.Services);
            Assert.Equal(service.Id, result.Services[0].Id);

            _hallRepositoryMock.Verify(
                repository => repository.Update(hall),
                Times.Once);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що при оновленні неіснуючого залу
        /// викидається KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_WithMissingHall_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            var request = new UpdateHallRequest
            {
                Name = "Updated Hall",
                Capacity = 100,
                HourlyRate = 2500
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hallId))
                .ReturnsAsync((Hall?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.UpdateAsync(hallId, request));

            _hallRepositoryMock.Verify(
                repository => repository.Update(
                    It.IsAny<Hall>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що при оновленні залу з неіснуючою послугою
        /// викидається KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_WithMissingService_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            var requestedServiceId = Guid.NewGuid();

            var request = new UpdateHallRequest
            {
                Name = "Updated Hall",
                Capacity = 100,
                HourlyRate = 2500,
                ServiceIds = [requestedServiceId]
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service>());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.UpdateAsync(
                    hall.Id,
                    request));

            _hallRepositoryMock.Verify(
                repository => repository.Update(
                    It.IsAny<Hall>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що існуючий зал без бронювань
        /// успішно видаляється.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_WithValidHall_ShouldDeleteHall()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(repository => repository.GetByHallAsync(hall.Id))
                .ReturnsAsync(new List<Booking>());

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            await _service.DeleteAsync(hall.Id);

            // Assert
            _hallRepositoryMock.Verify(
                repository => repository.Delete(hall),
                Times.Once);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що при видаленні неіснуючого залу
        /// викидається KeyNotFoundException.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_WithMissingHall_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var hallId = Guid.NewGuid();

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hallId))
                .ReturnsAsync((Hall?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.DeleteAsync(hallId));

            _bookingRepositoryMock.Verify(
                repository => repository.GetByHallAsync(
                    It.IsAny<Guid>()),
                Times.Never);

            _hallRepositoryMock.Verify(
                repository => repository.Delete(
                    It.IsAny<Hall>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що зал із існуючими бронюваннями
        /// не може бути видалений.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_WithExistingBookings_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            var booking = Booking.Create(
                hall.Id,
                new DateTime(2026, 8, 10, 10, 0, 0),
                new DateTime(2026, 8, 10, 12, 0, 0));

            _hallRepositoryMock
                .Setup(repository => repository.GetByIdAsync(hall.Id))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(repository => repository.GetByHallAsync(hall.Id))
                .ReturnsAsync(new List<Booking> { booking });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.DeleteAsync(hall.Id));

            _hallRepositoryMock.Verify(
                repository => repository.Delete(
                    It.IsAny<Hall>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що GetAvailableAsync повертає
        /// правильно перетворений список доступних залів.
        /// </summary>
        [Fact]
        public async Task GetAvailableAsync_ShouldReturnMappedHalls()
        {
            // Arrange
            var service = Service.Create(
                "Projector",
                500);

            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            hall.AddService(service.Id);

            var request = new AvailableHallsRequest
            {
                StartTime = new DateTime(2026, 8, 10, 10, 0, 0),
                EndTime = new DateTime(2026, 8, 10, 12, 0, 0),
                Capacity = 50
            };

            _hallRepositoryMock
                .Setup(repository => repository.GetAvailableAsync(
                    request.StartTime,
                    request.EndTime,
                    request.Capacity))
                .ReturnsAsync(new List<Hall> { hall });

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service> { service });

            // Act
            var result = await _service.GetAvailableAsync(request);

            // Assert
            var resultHall = Assert.Single(result);

            Assert.Equal(hall.Id, resultHall.Id);
            Assert.Equal("Conference Hall", resultHall.Name);
            Assert.Equal(100, resultHall.Capacity);
            Assert.Equal(2000, resultHall.HourlyRate);

            var resultService = Assert.Single(resultHall.Services);

            Assert.Equal(service.Id, resultService.Id);
            Assert.Equal("Projector", resultService.Name);
            Assert.Equal(500, resultService.Price);
        }

        /// <summary>
        /// Метод перевіряє, що GetAllAsync повертає
        /// правильно перетворений список залів.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedHalls()
        {
            // Arrange
            var service = Service.Create(
                "Wi-Fi",
                300);

            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            hall.AddService(service.Id);

            _hallRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Hall> { hall });

            _serviceRepositoryMock
                .Setup(repository => repository.GetByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Service> { service });

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            var resultHall = Assert.Single(result);

            Assert.Equal(hall.Id, resultHall.Id);
            Assert.Equal("Conference Hall", resultHall.Name);
            Assert.Equal(100, resultHall.Capacity);
            Assert.Equal(2000, resultHall.HourlyRate);

            var resultService = Assert.Single(resultHall.Services);

            Assert.Equal(service.Id, resultService.Id);
            Assert.Equal("Wi-Fi", resultService.Name);
            Assert.Equal(300, resultService.Price);
        }
    }
}