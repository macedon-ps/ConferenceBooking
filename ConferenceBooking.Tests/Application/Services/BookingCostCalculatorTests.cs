using ConferenceBooking.Application.Services;

namespace ConferenceBooking.Tests.Application.Services
{
    public class BookingCostCalculatorTests
    {
        private readonly BookingCostCalculator _calculator = new();

        /// <summary>
        /// Метод перевіряє розрахунок вартості бронювання
        /// у стандартній тарифній зоні з коефіцієнтом 1.00.
        /// </summary>
        [Fact]
        public void Calculate_DuringStandardHours_ShouldCalculateStandardCost()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 10, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 12, 0, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            Assert.Equal(4000m, result.HallCost);
            Assert.Equal(4000m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє застосування ранкового тарифу
        /// з коефіцієнтом 0.90.
        /// </summary>
        [Fact]
        public void Calculate_DuringMorningHours_ShouldApplyMorningDiscount()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 6, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 9, 0, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            Assert.Equal(5400m, result.HallCost);
            Assert.Equal(5400m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє застосування пікового тарифу
        /// з коефіцієнтом 1.15.
        /// </summary>
        [Fact]
        public void Calculate_DuringPeakHours_ShouldApplyPeakRate()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 12, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 14, 0, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            Assert.Equal(4600m, result.HallCost);
            Assert.Equal(4600m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє застосування вечірнього тарифу
        /// зі знижкою 20 відсотків та коефіцієнтом 0.80.
        /// </summary>
        [Fact]
        public void Calculate_DuringEveningHours_ShouldApplyEveningDiscount()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 18, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 21, 0, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            Assert.Equal(4800m, result.HallCost);
            Assert.Equal(4800m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє правильність розрахунку при переході
        /// зі стандартної тарифної зони до пікової.
        /// </summary>
        [Fact]
        public void Calculate_AcrossStandardAndPeakHours_ShouldApplyBothRates()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 11, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 15, 0, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            // 11:00-12:00 = 2000
            // 12:00-14:00 = 2000 * 1.15 * 2 = 4600
            // 14:00-15:00 = 2000
            // Total = 8600
            Assert.Equal(8600m, result.HallCost);
            Assert.Equal(8600m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє правильність розрахунку при переході
        /// зі стандартної тарифної зони до вечірньої.
        /// </summary>
        [Fact]
        public void Calculate_AcrossStandardAndEveningHours_ShouldApplyBothRates()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 17, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 19, 0, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            // 17:00-18:00 = 2000
            // 18:00-19:00 = 2000 * 0.80 = 1600
            // Total = 3600
            Assert.Equal(3600m, result.HallCost);
            Assert.Equal(3600m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє, що вартість додаткових послуг
        /// додається до вартості оренди залу.
        /// </summary>
        [Fact]
        public void Calculate_WithServicesCost_ShouldAddServicesCostToTotal()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 10, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 12, 0, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 1500m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            Assert.Equal(4000m, result.HallCost);
            Assert.Equal(5500m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє правильність розрахунку вартості
        /// при неповній годині бронювання.
        /// </summary>
        [Fact]
        public void Calculate_WithPartialHour_ShouldCalculateProportionalCost()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 10, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 11, 30, 0);
            const decimal hourlyRate = 2000m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            // 1.5 hours * 2000 = 3000
            Assert.Equal(3000m, result.HallCost);
            Assert.Equal(3000m, result.TotalCost);
        }

        /// <summary>
        /// Метод перевіряє округлення вартості оренди залу
        /// до двох знаків після десяткової крапки.
        /// </summary>
        [Fact]
        public void Calculate_WithFractionalCost_ShouldRoundHallCostToTwoDecimalPlaces()
        {
            // Arrange
            var startTime = new DateTime(2026, 8, 10, 12, 0, 0);
            var endTime = new DateTime(2026, 8, 10, 13, 0, 0);
            const decimal hourlyRate = 100.01m;
            const decimal servicesCost = 0m;

            // Act
            var result = _calculator.Calculate(
                startTime,
                endTime,
                hourlyRate,
                servicesCost);

            // Assert
            Assert.Equal(115.01m, result.HallCost);
            Assert.Equal(115.01m, result.TotalCost);
        }
    }
}