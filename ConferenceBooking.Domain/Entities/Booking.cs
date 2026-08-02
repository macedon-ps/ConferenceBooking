using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Клас Booking представляє бронювання конференц-залу для проведення заходів. Він містить інформацію про час початку та закінчення бронювання, загальну вартість, пов'язані послуги та конференц-зал.
    /// </summary>
    public class Booking
    {
        private readonly List<BookingService> _services = new();

        /// <summary>
        /// Guid Id бронювання.
        /// </summary>  
        public Guid Id { get; private set; }

        /// <summary>
        /// Guid Id конференц-залу, який бронюється. Зовнішній ключ для зв'язку з таблицею Hall.
        /// </summary>
        public Guid HallId { get; private set; }

        /// <summary>
        /// Час початку бронювання.
        /// </summary>
        public DateTime StartTime { get; private set; }
        
        /// <summary>
        /// Час закінчення бронювання.
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// Загальна вартість бронювання, яка включає вартість використання конференц-залу та вартість замовлених послуг.
        /// </summary>
        public decimal TotalCost { get; private set; }

        /// <summary>
        /// Коллекція замовлених послуг для бронювання. Може не збігатися з послугами, які пропонує конференц-зал, оскільки користувач може замовити лише ті послуги, які йому потрібні.
        /// </summary>
        public IReadOnlyCollection<BookingService> Services => _services.AsReadOnly();

        private Booking(Guid id, Guid hallId, DateTime startTime, DateTime endTime)
        {
            Id = id;
            HallId = hallId;
            StartTime = startTime;
            EndTime = endTime;
            TotalCost = 0;
        }

        /// <summary>
        /// Метод для створення нового бронювання. Перевіряє, чи є Guid конференц-залу порожнім, а також чи є час початку бронювання меншим за час закінчення. Якщо перевірки проходять успішно, створюється новий об'єкт Booking з унікальним Guid Id.
        /// </summary>
        /// <param name="hallId">Ідентифікатор конференц-залу.</param>
        /// <param name="startTime">Час початку бронювання.</param>
        /// <param name="endTime">Час закінчення бронювання.</param>
        /// <returns>Новий екземпляр класу Booking.</returns>
        public static Booking Create(Guid hallId, DateTime startTime, DateTime endTime)
        {
            ValidateHallId(hallId);
            ValidateTimeRange(startTime, endTime);

            return new Booking(
                Guid.NewGuid(),
                hallId,
                startTime,
                endTime);
        }

        /// <summary>
        /// Метод для додавання послуги до бронювання. Перевіряє, чи є Guid послуги порожнім, а також чи вже додана ця послуга до бронювання. Якщо перевірки проходять успішно, створюється новий об'єкт BookingService та додається до колекції послуг бронювання.
        /// </summary>
        /// <param name="serviceId">Ідентифікатор послуги.</param>
        /// <exception cref="DomainException"></exception>
        public void AddService(Guid serviceId)
        {
            if (serviceId == Guid.Empty)
                throw new DomainException(
                    "Service ID cannot be empty.");

            if (_services.Any(x => x.ServiceId == serviceId))
                throw new DomainException(
                    "This service is already added to the booking.");

            _services.Add(BookingService.Create(Id, serviceId));
        }

        /// <summary>
        /// Метод для видалення послуги з бронювання. Перевіряє, чи існує послуга з вказаним Guid у колекції послуг бронювання. Якщо послуга не знайдена, викидається виключення DomainException. Якщо послуга знайдена, вона видаляється з колекції.
        /// </summary>
        /// <param name="serviceId">Ідентифікатор послуги.</param>
        /// <exception cref="DomainException"></exception>
        public void RemoveService(Guid serviceId)
        {
            var service = _services
                .FirstOrDefault(x => x.ServiceId == serviceId);

            if (service is null)
                throw new DomainException(
                    "This service is not included in the booking.");

            _services.Remove(service);
        }

        /// <summary>
        /// Метод для встановлення загальної вартості бронювання. Перевіряє, чи є вказана вартість від'ємною. Якщо вартість від'ємна, викидається виключення DomainException. Якщо перевірка проходить успішно, встановлюється нова загальна вартість бронювання.
        /// </summary>
        /// <param name="totalCost">Загальна вартість бронювання.</param>
        /// <exception cref="DomainException"></exception>
        public void SetTotalCost(decimal totalCost)
        {
            if (totalCost < 0)
                throw new DomainException(
                    "Total cost cannot be negative.");

            TotalCost = totalCost;
        }

        /// <summary>
        /// Метод для перевірки, чи є Guid конференц-залу порожнім. Якщо Guid порожній, викидається виключення DomainException.
        /// </summary>
        /// <param name="hallId"></param>
        /// <exception cref="DomainException"></exception>
        private static void ValidateHallId(Guid hallId)
        {
            if (hallId == Guid.Empty)
                throw new DomainException(
                    "Hall ID cannot be empty.");
        }

        /// <summary>
        /// Метод для перевірки, чи є час початку бронювання меншим за час закінчення. Якщо час початку більший або рівний часу закінчення, викидається виключення DomainException.
        /// </summary>
        /// <param name="startTime">Час початку бронювання.</param>
        /// <param name="endTime">Час закінчення бронювання.</param>
        /// <exception cref="DomainException"></exception>
        private static void ValidateTimeRange(DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
                throw new DomainException(
                    "Booking start time must be earlier than end time.");
        }
    }
}

