using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Tests.Domain.Entities
{
    public class ServiceTests
    {
        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Service з валідними даними
        /// об'єкт створюється успішно.
        /// </summary>
        [Fact]
        public void Create_WithValidData_ShouldCreateService()
        {
            // Arrange
            const string name = "Projector";
            const decimal price = 500;

            // Act
            var service = Service.Create(
                name,
                price);

            // Assert
            Assert.NotEqual(Guid.Empty, service.Id);
            Assert.Equal(name, service.Name);
            Assert.Equal(price, service.Price);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Service з порожнім ім'ям
        /// викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithEmptyName_ShouldThrowDomainException()
        {
            // Act & Assert
            Assert.Throws<DomainException>(() =>
                Service.Create("", 500));
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Service з ім'ям,
        /// яке містить пробіли на початку та в кінці, пробіли видаляються.
        /// </summary>
        [Fact]
        public void Create_WithNameContainingWhitespace_ShouldTrimName()
        {
            // Arrange
            const string name = "  Projector  ";

            // Act
            var service = Service.Create(
                name,
                500);

            // Assert
            Assert.Equal("Projector", service.Name);
        }

        /// <summary>
        /// Метод перевіряє, що при створенні об'єкта Service
        /// з від'ємною ціною викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void Create_WithNegativePrice_ShouldThrowDomainException()
        {
            // Act & Assert
            Assert.Throws<DomainException>(() =>
                Service.Create("Projector", -100));
        }

        /// <summary>
        /// Метод перевіряє, що при зміні імені на валідне значення
        /// об'єкт Service успішно оновлює своє ім'я.
        /// </summary>
        [Fact]
        public void ChangeName_WithValidValue_ShouldChangeName()
        {
            // Arrange
            var service = Service.Create(
                "Projector",
                500);

            // Act
            service.Rename("Sound System");

            // Assert
            Assert.Equal("Sound System", service.Name);
        }

        /// <summary>
        /// Метод перевіряє, що при зміні імені на значення,
        /// яке містить пробіли на початку та в кінці, пробіли видаляються.
        /// </summary>
        [Fact]
        public void ChangeName_WithNameContainingWhitespace_ShouldTrimName()
        {
            // Arrange
            var service = Service.Create(
                "Projector",
                500);

            // Act
            service.Rename("  Sound System  ");

            // Assert
            Assert.Equal("Sound System", service.Name);
        }

        /// <summary>
        /// Метод перевіряє, що при зміні імені на порожнє значення
        /// викидається виключення DomainException.
        /// </summary>
        [Fact]
        public void ChangeName_WithEmptyName_ShouldThrowDomainException()
        {
            // Arrange
            var service = Service.Create(
                "Projector",
                500);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                service.Rename(""));
        }

        /// <summary>
        /// Метод перевіряє, що при зміні ціни на валідне значення
        /// об'єкт Service успішно оновлює свою ціну.
        /// </summary>
        [Fact]
        public void ChangePrice_WithValidValue_ShouldChangePrice()
        {
            // Arrange
            var service = Service.Create(
                "Projector",
                500);

            // Act
            service.ChangePrice(700);

            // Assert
            Assert.Equal(700, service.Price);
        }

        /// <summary>
        /// Метод перевіряє, що при зміні ціни на від'ємне значення
        /// викидається виключение DomainException.
        /// </summary>
        [Fact]
        public void ChangePrice_WithNegativeValue_ShouldThrowDomainException()
        {
            // Arrange
            var service = Service.Create(
                "Projector",
                500);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                service.ChangePrice(-100));
        }
    }
}