using ConferenceBooking.Application.DTOs.Bookings;
using ConferenceBooking.Application.DTOs.Services;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConferenceBooking.Application.Services;

/// <summary>
/// Клас сервісу для управління бронюваннями конференцій. Реалізує інтерфейс IBookingApplicationService та надає методи для створення бронювань, перевірки конфліктів та валідації послуг.
/// </summary>
public class BookingApplicationService : IBookingApplicationService
{
    /// <summary>
    /// Репозиторій для роботи з бронюваннями. Використовується для доступу до даних про бронювання та перевірки конфліктів у часі.
    /// </summary>
    private readonly IBookingRepository _bookingRepository;

    /// <summary>
    /// Репозиторій для роботи з залами. Використовується для доступу до даних про зали та перевірки доступності послуг у конкретному залі.
    /// </summary>
    private readonly IHallRepository _hallRepository;

    /// <summary>
    /// Репозиторій для роботи з послугами. Використовується для доступу до даних про послуги та перевірки їх наявності.
    /// </summary>
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Репозиторій для управління транзакціями та збереження змін у базі даних. Використовується для забезпечення цілісності даних під час створення бронювань та додавання послуг.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Логер для запису повідомлень про помилки та інформаційних повідомлень. Використовується для логування подій під час виконання операцій.
    /// </summary>
    private readonly ILogger<BookingApplicationService> _logger;

    /// <summary>
    /// Конструктор класу BookingApplicationService, який приймає репозиторії та об'єкт UnitOfWork як параметри. Ініціалізує приватні поля для доступу до даних про бронювання, зали та послуги.
    /// </summary>
    /// <param name="bookingRepository">Репозиторій для роботи з бронюваннями</param>
    /// <param name="hallRepository">Репозиторій для роботи з залами</param>
    /// <param name="serviceRepository">Репозиторій для роботи з послугами</param>
    /// <param name="unitOfWork">Об'єкт UnitOfWork для управління транзакціями</param>
    public BookingApplicationService(IBookingRepository bookingRepository, IHallRepository hallRepository, IServiceRepository serviceRepository, IUnitOfWork unitOfWork, ILogger<BookingApplicationService> logger)
    {
        _bookingRepository = bookingRepository;
        _hallRepository = hallRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Метод для отримання всіх бронювань. Використовується для отримання списку всіх бронювань у системі. Повертає колекцію об'єктів BookingResponse з інформацією про бронювання та пов'язані послуги.
    /// </summary>
    /// <returns>Колекція об'єктів BookingResponse з інформацією про бронювання та пов'язані послуги</returns>
    public async Task<IReadOnlyCollection<BookingResponse>> GetAllAsync()
    {
        var bookings = await _bookingRepository
            .GetAllAsync();

        var serviceIds = bookings
            .SelectMany(b => b.Services)
            .Select(bs => bs.ServiceId)
            .Distinct()
            .ToList();

        var services = await _serviceRepository
            .GetByIdsAsync(serviceIds);

        return bookings
            .Select(booking => MapToResponse(
                booking,
                services))
            .ToList();
    }

    /// <summary>
    /// Метод для отримання бронювання за його ідентифікатором. Перевіряє наявність бронювання у базі даних, а якщо бронювання не знайдено, викидає KeyNotFoundException. Повертає об'єкт BookingResponse з інформацією про бронювання та пов'язані послуги.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання</param>
    /// <returns>Об'єкт BookingResponse з інформацією про бронювання та пов'язані послуги</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<BookingResponse> GetByIdAsync(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
        {
            _logger.LogWarning("Booking retrieval failed. Booking not found. BookingId: {BookingId}.", id);

            throw new KeyNotFoundException($"Booking with ID '{id}' was not found.");
        }

        var serviceIds = booking.Services
            .Select(bs => bs.ServiceId)
            .Distinct()
            .ToList();

        var services = await _serviceRepository
            .GetByIdsAsync(serviceIds);

        return MapToResponse(booking, services);
    }

    /// <summary>
    /// Метод для отримання всіх бронювань, пов'язаних з конкретним залом. Використовується для отримання списку бронювань за ідентифікатором залу. Повертає колекцію об'єктів BookingResponse з інформацією про бронювання та пов'язані послуги.
    /// </summary>
    /// <param name="hallId">Унікальний ідентифікатор залу</param>
    /// <returns>Колекція об'єктів BookingResponse з інформацією про бронювання та пов'язані послуги</returns>
    public async Task<IReadOnlyCollection<BookingResponse>> GetByHallAsync(Guid hallId)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);

        if (hall is null)
        {
            throw new KeyNotFoundException($"Hall with ID '{hallId}' was not found.");
        }

        var bookings = await _bookingRepository
            .GetByHallAsync(hallId);

        var serviceIds = bookings
            .SelectMany(b => b.Services)
            .Select(bs => bs.ServiceId)
            .Distinct()
            .ToList();

        var services = await _serviceRepository
            .GetByIdsAsync(serviceIds);

        return bookings
            .Select(booking => MapToResponse(
                booking,
                services))
            .ToList();
    }

    /// <summary>
    /// Метод для створення нового бронювання. Перевіряє наявність залу, конфлікти у часі та доступність послуг, а також обчислює загальну вартість бронювання. Повертає об'єкт BookingResponse з інформацією про створене бронювання.
    /// </summary>
    /// <param name="request">Об'єкт CreateBookingRequest, що містить дані для створення бронювання</param>
    /// <returns>Об'єкт BookingResponse з інформацією про створене бронювання</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        var hall = await _hallRepository
            .GetByIdAsync(request.HallId);

        if (hall is null)
        {
            _logger.LogWarning("Booking creation failed. Hall not found. HallId: {HallId}.", request.HallId);

            throw new KeyNotFoundException($"Hall with ID '{request.HallId}' was not found.");
        }

        var hasConflict = await _bookingRepository
            .HasConflictAsync(
                request.HallId,
                request.StartTime,
                request.EndTime);

        if (hasConflict)
        {
            _logger.LogWarning("Booking creation rejected because the hall is already booked. HallId: {HallId}, StartTime: {StartTime}, EndTime: {EndTime}.", request.HallId, request.StartTime, request.EndTime);

            throw new InvalidOperationException("The hall is already booked for the selected time period.");
        }

        var serviceIds = request.ServiceIds
            .Distinct()
            .ToList();

        var services = await _serviceRepository
            .GetByIdsAsync(serviceIds);

        ValidateServicesExist(serviceIds, services);

        ValidateHallServices(
            hall,
            serviceIds);

        var booking = Booking.Create(
            request.HallId,
            request.StartTime,
            request.EndTime);

        foreach (var service in services)
        {
            booking.AddService(service.Id);
        }

        /*
         * На этом этапе полноценный расчет стоимости
         * еще не реализован.
         *
         * Шаг 13 проекта — отдельная реализация
         * расчета стоимости с учетом тарифных периодов.
         *
         * Пока сохраняем базовую стоимость:
         * стоимость зала за час × количество часов
         * + стоимость выбранных услуг.
         */
        var durationInHours =
            (decimal)(request.EndTime - request.StartTime)
            .TotalHours;

        var hallCost =
            hall.HourlyRate * durationInHours;

        var servicesCost =
            services.Sum(service => service.Price);

        var totalCost =
            hallCost + servicesCost;

        booking.SetTotalCost(totalCost);

        await _bookingRepository.AddAsync(booking);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Booking created successfully. BookingId: {BookingId}, HallId: {HallId}, StartTime: {StartTime}, EndTime: {EndTime}, TotalCost: {TotalCost}.", booking.Id, booking.HallId, booking.StartTime, booking.EndTime, booking.TotalCost);

        return MapToResponse(
            booking,
            services);
    }

    /// <summary>
    /// Метод для перевірки наявності всіх запитаних послуг у базі даних. Якщо будь-яка з послуг не знайдена, метод викидає KeyNotFoundException з переліком відсутніх ідентифікаторів послуг.
    /// </summary>
    /// <param name="requestedIds">Колекція ідентифікаторів запитаних послуг</param>
    /// <param name="foundServices">Колекція знайдених об'єктів Service</param>
    /// <exception cref="KeyNotFoundException"></exception>
    private void ValidateServicesExist(IReadOnlyCollection<Guid> requestedIds, IReadOnlyCollection<Service> foundServices)
    {
        var foundIds = foundServices
            .Select(service => service.Id)
            .ToHashSet();

        var missingIds = requestedIds
            .Where(id => !foundIds.Contains(id))
            .ToList();

        if (missingIds.Count > 0)
        {
            _logger.LogWarning(
                "Booking creation failed. Services not found. ServiceIds: {ServiceIds}.",
                missingIds);

            throw new KeyNotFoundException(
                $"Services not found: {string.Join(", ", missingIds)}.");
        }
    }

    /// <summary>
    /// Метод для перевірки доступності запитаних послуг у конкретному залі. Якщо будь-яка з послуг недоступна в залі, метод викидає InvalidOperationException з переліком недоступних ідентифікаторів послуг.
    /// </summary>
    /// <param name="hall">Об'єкт Hall, у якому перевіряється доступність послуг</param>
    /// <param name="requestedServiceIds">Колекція ідентифікаторів запитаних послуг</param>
    /// <exception cref="InvalidOperationException"></exception>
    private void ValidateHallServices(Hall hall, IReadOnlyCollection<Guid> requestedServiceIds)
    {
        var availableServiceIds = hall.Services
            .Select(service => service.ServiceId)
            .ToHashSet();

        var unavailableServiceIds = requestedServiceIds
            .Where(id => !availableServiceIds.Contains(id))
            .ToList();

        if (unavailableServiceIds.Count > 0)
        {
            _logger.LogWarning(
                "Booking creation rejected. Requested services are not available in hall. HallId: {HallId}, HallName: {HallName}, ServiceIds: {ServiceIds}.",
                hall.Id,
                hall.Name,
                unavailableServiceIds);

            throw new InvalidOperationException(
                $"The following services are not available in hall " +
                $"'{hall.Name}': " +
                $"{string.Join(", ", unavailableServiceIds)}.");
        }
    }

    /// <summary>
    /// Метод для перетворення об'єкта Booking та колекції об'єктів Service у об'єкт BookingResponse. Використовується для формування відповіді після створення бронювання.
    /// </summary>
    /// <param name="booking">Об'єкт Booking, який потрібно перетворити</param>
    /// <param name="services">Колекція об'єктів Service, пов'язаних з бронюванням</param>
    /// <returns>Об'єкт BookingResponse, що представляє результат бронювання</returns>
    private static BookingResponse MapToResponse(Booking booking, IReadOnlyCollection<Service> services)
    {
        var serviceDictionary = services
            .ToDictionary(service => service.Id);

        return new BookingResponse
        {
            Id = booking.Id,
            HallId = booking.HallId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            TotalCost = booking.TotalCost,

            Services = booking.Services
                .Select(bookingService =>
                {
                    var service =
                        serviceDictionary[bookingService.ServiceId];

                    return new ServiceResponse
                    {
                        Id = service.Id,
                        Name = service.Name,
                        Price = service.Price
                    };
                })
                .ToList()
        };
    }

    /// <summary>
    /// Метод для видалення бронювання за його ідентифікатором. Перевіряє наявність бронювання у базі даних, а якщо бронювання не знайдено, викидає KeyNotFoundException. Після видалення бронювання зберігає зміни у базі даних.
    /// </summary>
    /// <param name="id">Ідентифікатор бронювання, яке потрібно видалити</param>
    /// <returns>Завершення завдання без повернення значення</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task DeleteAsync(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
        {
            _logger.LogWarning("Booking deletion failed. Booking not found. BookingId: {BookingId}.", id);

            throw new KeyNotFoundException(
                $"Booking with ID '{id}' was not found.");
        }

        _bookingRepository.Delete(booking);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Booking deleted successfully. BookingId: {BookingId}, HallId: {HallId}.", booking.Id, booking.HallId);
    }
}