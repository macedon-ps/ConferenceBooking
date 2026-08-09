using ConferenceBooking.Application.Services;
using ConferenceBooking.Domain.Exceptions;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Domain.Models.Reports;
using Moq;

namespace ConferenceBooking.Tests.Application.Services
{
    public class ReportApplicationServiceTests
    {
        private readonly Mock<IReportRepository> _reportRepositoryMock;
        private readonly ReportApplicationService _service;

        public ReportApplicationServiceTests()
        {
            _reportRepositoryMock = new Mock<IReportRepository>();

            _service = new ReportApplicationService(
                _reportRepositoryMock.Object);
        }

        /// <summary>
        /// Метод перевіряє, що при валідному періоді
        /// GetBookingSummaryAsync повертає правильно сформовану статистику.
        /// </summary>
        [Fact]
        public async Task GetBookingSummaryAsync_WithValidPeriod_ShouldReturnSummary()
        {
            // Arrange
            var from = new DateTime(2026, 8, 1);
            var to = new DateTime(2026, 8, 10);

            var model = new BookingSummaryModel
            {
                TotalBookings = 10,
                TotalBookedHours = 25.5m,
                TotalRevenue = 50000m,
                AverageBookingCost = 5000m
            };

            _reportRepositoryMock
                .Setup(repository => repository.GetBookingSummaryAsync(from, to))
                .ReturnsAsync(model);

            // Act
            var result = await _service.GetBookingSummaryAsync(from, to);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.TotalBookings);
            Assert.Equal(25.5m, result.TotalBookedHours);
            Assert.Equal(50000m, result.TotalRevenue);
            Assert.Equal(5000m, result.AverageBookingCost);

            _reportRepositoryMock.Verify(
                repository => repository.GetBookingSummaryAsync(from, to),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що при валідному періоді
        /// GetHallUtilizationAsync правильно перетворює моделі у DTO.
        /// </summary>
        [Fact]
        public async Task GetHallUtilizationAsync_WithValidPeriod_ShouldReturnHallUtilization()
        {
            // Arrange
            var from = new DateTime(2026, 8, 1);
            var to = new DateTime(2026, 8, 10);

            var hallId = Guid.NewGuid();

            var models = new List<HallUtilizationModel>
            {
                new HallUtilizationModel
                {
                    HallId = hallId,
                    HallName = "Conference Hall",
                    BookingCount = 5,
                    TotalBookedHours = 12.5m,
                    TotalRevenue = 25000m
                },
                new HallUtilizationModel
                {
                    HallId = Guid.NewGuid(),
                    HallName = "Meeting Hall",
                    BookingCount = 3,
                    TotalBookedHours = 7m,
                    TotalRevenue = 12000m
                }
            };

            _reportRepositoryMock
                .Setup(repository => repository.GetHallUtilizationAsync(from, to))
                .ReturnsAsync(models);

            // Act
            var result = await _service.GetHallUtilizationAsync(from, to);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var firstHall = result.First();

            Assert.Equal(hallId, firstHall.HallId);
            Assert.Equal("Conference Hall", firstHall.HallName);
            Assert.Equal(5, firstHall.BookingCount);
            Assert.Equal(12.5m, firstHall.TotalBookedHours);
            Assert.Equal(25000m, firstHall.TotalRevenue);

            _reportRepositoryMock.Verify(
                repository => repository.GetHallUtilizationAsync(from, to),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що при валідному періоді
        /// GetPopularServicesAsync правильно перетворює моделі у DTO.
        /// </summary>
        [Fact]
        public async Task GetPopularServicesAsync_WithValidPeriod_ShouldReturnPopularServices()
        {
            // Arrange
            var from = new DateTime(2026, 8, 1);
            var to = new DateTime(2026, 8, 10);

            var serviceId = Guid.NewGuid();

            var models = new List<PopularServiceModel>
            {
                new PopularServiceModel
                {
                    ServiceId = serviceId,
                    ServiceName = "Projector",
                    UsageCount = 8
                },
                new PopularServiceModel
                {
                    ServiceId = Guid.NewGuid(),
                    ServiceName = "Sound",
                    UsageCount = 5
                }
            };

            _reportRepositoryMock
                .Setup(repository => repository.GetPopularServicesAsync(from, to))
                .ReturnsAsync(models);

            // Act
            var result = await _service.GetPopularServicesAsync(from, to);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var firstService = result.First();

            Assert.Equal(serviceId, firstService.ServiceId);
            Assert.Equal("Projector", firstService.ServiceName);
            Assert.Equal(8, firstService.UsageCount);

            _reportRepositoryMock.Verify(
                repository => repository.GetPopularServicesAsync(from, to),
                Times.Once);
        }

        /// <summary>
        /// Метод перевіряє, що при некоректному періоді
        /// GetBookingSummaryAsync викидає DomainException.
        /// </summary>
        [Fact]
        public async Task GetBookingSummaryAsync_WithInvalidPeriod_ShouldThrowDomainException()
        {
            // Arrange
            var from = new DateTime(2026, 8, 10);
            var to = new DateTime(2026, 8, 1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _service.GetBookingSummaryAsync(from, to));

            Assert.Equal(
                "The 'from' date must be earlier than the 'to' date.",
                exception.Message);

            _reportRepositoryMock.Verify(
                repository => repository.GetBookingSummaryAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що при некоректному періоді
        /// GetHallUtilizationAsync викидає DomainException.
        /// </summary>
        [Fact]
        public async Task GetHallUtilizationAsync_WithInvalidPeriod_ShouldThrowDomainException()
        {
            // Arrange
            var from = new DateTime(2026, 8, 10);
            var to = new DateTime(2026, 8, 1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _service.GetHallUtilizationAsync(from, to));

            Assert.Equal(
                "The 'from' date must be earlier than the 'to' date.",
                exception.Message);

            _reportRepositoryMock.Verify(
                repository => repository.GetHallUtilizationAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()),
                Times.Never);
        }

        /// <summary>
        /// Метод перевіряє, що при некоректному періоді
        /// GetPopularServicesAsync викидає DomainException.
        /// </summary>
        [Fact]
        public async Task GetPopularServicesAsync_WithInvalidPeriod_ShouldThrowDomainException()
        {
            // Arrange
            var from = new DateTime(2026, 8, 10);
            var to = new DateTime(2026, 8, 1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _service.GetPopularServicesAsync(from, to));

            Assert.Equal(
                "The 'from' date must be earlier than the 'to' date.",
                exception.Message);

            _reportRepositoryMock.Verify(
                repository => repository.GetPopularServicesAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()),
                Times.Never);
        }
    }
}