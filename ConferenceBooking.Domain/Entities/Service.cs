using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Класс Service представляє послугу, яку можна замовити разом із бронюванням конференц-залу. Він містить інформацію про назву послуги, її ціну та пов'язаний конференц-зал.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Guid Id послуги.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Назва послуги.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Вартість послуги.
        /// </summary>
        public decimal Price { get; private set; }

        private Service(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        /// <summary>
        /// Метод для створення нової послуги з валідацією вхідних даних. Якщо назва порожня або ціна від'ємна, буде згенеровано виняток DomainException.
        /// </summary>
        /// <param name="name">Назва послуги.</param>
        /// <param name="price">Вартість послуги.</param>
        /// <returns>Новий екземпляр послуги.</returns>
        public static Service Create(string name, decimal price)
        {
            ValidateName(name);
            ValidatePrice(price);

            return new Service(
                Guid.NewGuid(),
                name.Trim(),
                price);
        }

        /// <summary>
        /// Метод для перейменування послуги з валідацією нового імені. Якщо нова назва порожня, буде згенеровано виняток DomainException.
        /// </summary>
        /// <param name="newName">Нова назва послуги.</param>
        public void Rename(string newName)
        {
            ValidateName(newName);

            Name = newName.Trim();
        }

        /* Метод може виористовуватись в майбутньому, якщо виникне потреба зміни ціни послуги */
        /// <summary>
        /// Метод для зміни ціни послуги з валідацією нової ціни. Якщо нова ціна від'ємна, буде згенеровано виняток DomainException.
        /// </summary>
        /// <param name="newPrice">Нова вартість послуги.</param>
        public void ChangePrice(decimal newPrice)
        {
            ValidatePrice(newPrice);

            Price = newPrice;
        }

        /// <summary>
        /// Метод для валідації назви послуги. Якщо назва порожня або складається лише з пробілів, буде згенеровано виняток DomainException.
        /// </summary>
        /// <param name="name">Назва послуги.</param>
        /// <exception cref="DomainException"></exception>
        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(
                    "Service name cannot be empty.");
        }

        /// <summary>
        /// Метод для валідації ціни послуги. Якщо ціна від'ємна, буде згенеровано виняток DomainException.
        /// </summary>
        /// <param name="price">Вартість послуги.</param>
        /// <exception cref="DomainException"></exception>
        private static void ValidatePrice(decimal price)
        {
            if (price < 0)
                throw new DomainException(
                    "Service price cannot be negative.");
        }
    }
}
