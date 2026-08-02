using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Tests.Domain.Entities
{
    public class HallTests
    {
        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Hall з валідними даними об'єкт створюється успішно.
        /// </summary>
        [Fact]
        public void Create_WithValidData_ShouldCreateHall()
        {
            // Arrange
            const string name = "Conference Hall";
            const int capacity = 100;
            const decimal hourlyRate = 2000;

            // Act
            var hall = Hall.Create(
                name,
                capacity,
                hourlyRate);

            // Assert
            Assert.NotEqual(Guid.Empty, hall.Id);
            Assert.Equal(name, hall.Name);
            Assert.Equal(capacity, hall.Capacity);
            Assert.Equal(hourlyRate, hall.HourlyRate);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Hall з порожнім ім'ям викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEmptyName_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() =>
                Hall.Create("", 100, 2000));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Hall з нульовою місткістю викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithZeroCapacity_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() =>
                Hall.Create("Conference Hall", 0, 2000));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Hall з від'ємною місткістю викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithNegativeCapacity_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() =>
                Hall.Create("Conference Hall", -1, 2000));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Hall з від'ємною погодинною ставкою викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithNegativeHourlyRate_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() =>
                Hall.Create("Conference Hall", 100, -1));
        }

        /// <summary>
        /// Метод перевіряє, що при зміні погодинної ставки на валідне значення об'єкт Hall успішно оновлює свою ставку.
        /// </summary>
        [Fact]
        public void ChangeHourlyRate_WithValidValue_ShouldChangeRate()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            // Act
            hall.ChangeHourlyRate(2500);

            // Assert
            Assert.Equal(2500, hall.HourlyRate);
        }

        /// <summary>
        /// Метод перевіряє, що при зміні погодинної ставки на від'ємне значення викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void ChangeHourlyRate_WithNegativeValue_ShouldThrowDomainException()
        {
            // Arrange
            var hall = Hall.Create(
                "Conference Hall",
                100,
                2000);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                hall.ChangeHourlyRate(-100));
        }
    }
}
