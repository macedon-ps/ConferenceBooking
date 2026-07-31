using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Клас Hall представляє конференц-зал, який можна забронювати для проведення заходів. Він містить інформацію про назву залу, його місткість, погодинну ставку та пов'язані послуги та бронювання.
    /// </summary>
    public class Hall
    {
        private readonly List<HallService> _services = new();

        /// <summary>
        /// Guid Id для конференц-залу.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Назва конференц-залу.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Місткість конференц-залу.
        /// </summary>
        public int Capacity { get; private set; }
        
        /// <summary>
        /// Погодинна ставка оплати за використання конференц-залу.
        /// </summary>
        public decimal HourlyRate { get; private set; }

        /// <summary>
        /// Коллекція запропонованих послуг конференц-залу.
        /// </summary>
        public IReadOnlyCollection<HallService> Services => _services.AsReadOnly();

        private Hall(Guid id, string name, int capacity, decimal hourlyRate)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
            HourlyRate = hourlyRate;
        }

        /// <summary>
        /// Метод створення нового об'єкта Hall. Виконує валідацію параметрів та повертає новий екземпляр конференц-залу.
        /// </summary>
        /// <param name="name">Назва конференц-залу.</param>
        /// <param name="capacity">Місткість конференц-залу.</param>
        /// <param name="hourlyRate">Погодинна ставка оплати за використання конференц-залу.</param>
        /// <returns>Новий екземпляр конференц-залу.</returns>
        public static Hall Create(string name, int capacity, decimal hourlyRate)
        {
            ValidateName(name);
            ValidateCapacity(capacity);
            ValidateHourlyRate(hourlyRate);

            return new Hall(
                Guid.NewGuid(),
                name.Trim(),
                capacity,
                hourlyRate);
        }

        /// <summary>
        /// Метод оновлення інформації про конференц-зал. Виконує валідацію параметрів та оновлює відповідні властивості об'єкта.
        /// </summary>
        /// <param name="name">Назва конференц-залу.</param>
        /// <param name="capacity">Місткість конференц-залу.</param>
        /// <param name="hourlyRate">Погодинна ставка оплати за використання конференц-залу.</param>
        public void Update(string name, int capacity, decimal hourlyRate)
        {
            ValidateName(name);
            ValidateCapacity(capacity);
            ValidateHourlyRate(hourlyRate);

            Name = name.Trim();
            Capacity = capacity;
            HourlyRate = hourlyRate;
        }

        /// <summary>
        /// Метод зміни погодинної ставки конференц-залу. Виконує валідацію нового значення та оновлює відповідну властивість об'єкта.
        /// </summary>
        /// <param name="newHourlyRate">Нова погодинна ставка оплати за використання конференц-залу.</param>
        public void ChangeHourlyRate(decimal newHourlyRate)
        {
            ValidateHourlyRate(newHourlyRate);

            HourlyRate = newHourlyRate;
        }

        /// <summary>
        /// Метод додавання послуги до конференц-залу. Перевіряє, чи не є послуга вже доданою, та додає її до колекції послуг.
        /// </summary>
        /// <param name="serviceId">Ідентифікатор послуги.</param>
        /// <exception cref="DomainException"></exception>
        public void AddService(Guid serviceId)
        {
            if (serviceId == Guid.Empty)
                throw new DomainException("Service ID cannot be empty.");

            if (_services.Any(x => x.ServiceId == serviceId))
                throw new DomainException(
                    "This service is already available in the hall.");

            _services.Add(HallService.Create(Id, serviceId));
        }

        /// <summary>
        /// Метод видалення послуги з конференц-залу. Перевіряє, чи є послуга в колекції, та видаляє її. Якщо послуга не знайдена, викидає DomainException.
        /// </summary>
        /// <param name="serviceId">Ідентифікатор послуги.</param>
        /// <exception cref="DomainException"></exception>
        public void RemoveService(Guid serviceId)
        {
            var service = _services
                .FirstOrDefault(x => x.ServiceId == serviceId);

            if (service is null)
                throw new DomainException(
                    "This service is not available in the hall.");

            _services.Remove(service);
        }

        /// <summary>
        /// Метод валідації назви конференц-залу. Перевіряє, чи не є назва порожньою або лише пробілами. Якщо назва недійсна, викидає DomainException.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="DomainException"></exception>
        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Hall name cannot be empty.");
        }

        /// <summary>
        /// Метод валідації місткості конференц-залу. Перевіряє, чи є місткість додатнім числом. Якщо місткість недійсна, викидає DomainException.
        /// </summary>
        /// <param name="capacity"></param>
        /// <exception cref="DomainException"></exception>
        private static void ValidateCapacity(int capacity)
        {
            if (capacity <= 0)
                throw new DomainException(
                    "Hall capacity must be greater than zero.");
        }

        /// <summary>
        /// Метод валідації погодинної ставки конференц-залу. Перевіряє, чи не є погодинна ставка від'ємним числом. Якщо погодинна ставка недійсна, викидає DomainException.    
        /// </summary>
        /// <param name="hourlyRate"></param>
        /// <exception cref="DomainException"></exception>
        private static void ValidateHourlyRate(decimal hourlyRate)
        {
            if (hourlyRate < 0)
                throw new DomainException(
                    "Hourly rate cannot be negative.");
        }
    }
}
