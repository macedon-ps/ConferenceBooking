using ConferenceBooking.Application.DTOs.Halls;
using ConferenceBooking.Application.DTOs.Services;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;

namespace ConferenceBooking.Application.Services;

/// <summary>
/// Клас сервісу для управління залами конференцій. Містить методи для створення, оновлення, видалення та отримання доступних залів.
/// </summary>
public class HallApplicationService : IHallApplicationService
{
    /// <summary>
    /// Репозиторій для роботи з залами конференцій. Використовується для доступу до даних про зали та виконання операцій CRUD.
    /// </summary>
    private readonly IHallRepository _hallRepository;

    /// <summary>
    /// Репозиторій для роботи з послугами конференцій. Використовується для доступу до даних про послуги та виконання операцій CRUD.
    /// </summary>
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Репозиторій для управління транзакціями та збереження змін у базі даних. Використовується для забезпечення цілісності даних під час виконання операцій.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Конструктор класу HallApplicationService, який приймає репозиторії для залів, послуг та управління транзакціями як параметри.
    /// </summary>
    /// <param name="hallRepository">Репозиторій для роботи з залами конференцій</param>
    /// <param name="serviceRepository">Репозиторій для роботи з послугами конференцій</param>
    /// <param name="unitOfWork">Репозиторій для управління транзакціями та збереження змін у базі даних</param>
    public HallApplicationService(
        IHallRepository hallRepository,
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _hallRepository = hallRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Метод для створення нового залу конференцій. Приймає об'єкт CreateHallRequest, який містить дані про зал та список ідентифікаторів послуг, що надаються у залі. Перевіряє наявність послуг у базі даних, створює новий об'єкт Hall та зберігає його у базі даних.
    /// </summary>
    /// <param name="request">Об'єкт CreateHallRequest, що містить дані про зал та список ідентифікаторів послуг</param>
    /// <returns>Об'єкт HallResponse, що містить дані про створений зал та надані послуги</returns>
    public async Task<HallResponse> CreateAsync(CreateHallRequest request)
    {
        var serviceIds = request.ServiceIds
            .Distinct()
            .ToList();

        var services = await _serviceRepository
            .GetByIdsAsync(serviceIds);

        ValidateServices(serviceIds, services);

        var hall = Hall.Create(
            request.Name,
            request.Capacity,
            request.HourlyRate);

        foreach (var service in services)
        {
            hall.AddService(service.Id);
        }

        await _hallRepository.AddAsync(hall);

        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(hall, services);
    }

    /// <summary>
    /// Метод для оновлення існуючого залу конференцій. Приймає ідентифікатор залу та об'єкт UpdateHallRequest, який містить нові дані про зал та список ідентифікаторів послуг. Перевіряє наявність залу та послуг у базі даних, оновлює дані залу та зберігає зміни у базі даних.
    /// </summary>
    /// <param name="id">Ідентифікатор залу, який потрібно оновити</param>
    /// <param name="request">Об'єкт UpdateHallRequest, що містить нові дані про зал та список ідентифікаторів послуг</param>
    /// <returns>Об'єкт HallResponse, що містить дані про оновлений зал та надані послуги</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<HallResponse> UpdateAsync(Guid id, UpdateHallRequest request)
    {
        var hall = await _hallRepository.GetByIdAsync(id);

        if (hall is null)
        {
            throw new KeyNotFoundException(
                $"Hall with ID '{id}' was not found.");
        }

        var serviceIds = request.ServiceIds
            .Distinct()
            .ToList();

        var services = await _serviceRepository
            .GetByIdsAsync(serviceIds);

        ValidateServices(serviceIds, services);

        hall.Update(
            request.Name,
            request.Capacity,
            request.HourlyRate);

        foreach (var hallService in hall.Services.ToList())
        {
            hall.RemoveService(hallService.ServiceId);
        }

        foreach (var service in services)
        {
            hall.AddService(service.Id);
        }

        _hallRepository.Update(hall);

        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(hall, services);
    }

    /// <summary>
    /// Метод для видалення існуючого залу конференцій. Приймає ідентифікатор залу, перевіряє наявність залу у базі даних та видаляє його. Зміни зберігаються у базі даних.
    /// </summary>
    /// <param name="id">Ідентифікатор залу, який потрібно видалити</param>
    /// <returns>Завершення завдання без повернення значення</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task DeleteAsync(Guid id)
    {
        var hall = await _hallRepository.GetByIdAsync(id);

        if (hall is null)
        {
            throw new KeyNotFoundException(
                $"Hall with ID '{id}' was not found.");
        }

        _hallRepository.Delete(hall);

        await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Метод для отримання списку доступних залів конференцій на основі заданого проміжку часу та мінімальної місткості. Приймає об'єкт AvailableHallsRequest, який містить початковий та кінцевий час, а також мінімальну місткість. Повертає колекцію об'єктів HallResponse, що містять дані про доступні зали та надані послуги.
    /// </summary>
    /// <param name="request">Об'єкт AvailableHallsRequest, що містить початковий та кінцевий час, а також мінімальну місткість</param>
    /// <returns>Колекція об'єктів HallResponse, що містять дані про доступні зали та надані послуги</returns>
    public async Task<IReadOnlyCollection<HallResponse>> GetAvailableAsync(AvailableHallsRequest request)
    {
        var halls = await _hallRepository.GetAvailableAsync(
            request.StartTime,
            request.EndTime,
            request.Capacity);

        var serviceIds = halls
            .SelectMany(h => h.Services)
            .Select(hs => hs.ServiceId)
            .Distinct()
            .ToList();

        var services = await _serviceRepository
            .GetByIdsAsync(serviceIds);

        var serviceDictionary = services
            .ToDictionary(s => s.Id);

        return halls
            .Select(hall => MapToResponse(
                hall,
                serviceDictionary))
            .ToList();
    }

    /// <summary>
    /// Метод для перевірки наявності всіх запитаних послуг у базі даних. Приймає колекцію ідентифікаторів запитаних послуг та колекцію знайдених об'єктів Service. Якщо деякі з запитаних послуг не знайдені, метод викидає виняток KeyNotFoundException з повідомленням про відсутні послуги.
    /// </summary>
    /// <param name="requestedIds">Колекція ідентифікаторів запитаних послуг</param>
    /// <param name="foundServices">Колекція знайдених об'єктів Service</param>
    /// <exception cref="KeyNotFoundException"></exception>
    private static void ValidateServices(
        IReadOnlyCollection<Guid> requestedIds,
        IReadOnlyCollection<Service> foundServices)
    {
        var foundIds = foundServices
            .Select(s => s.Id)
            .ToHashSet();

        var missingIds = requestedIds
            .Where(id => !foundIds.Contains(id))
            .ToList();

        if (missingIds.Count > 0)
        {
            throw new KeyNotFoundException(
                $"Services not found: {string.Join(", ", missingIds)}.");
        }
    }

    /// <summary>
    /// Метод для перетворення об'єкта Hall та колекції об'єктів Service у об'єкт HallResponse. Створює словник послуг для швидкого доступу за ідентифікатором та викликає інший метод MapToResponse для створення об'єкта HallResponse.
    /// </summary>
    /// <param name="hall">Об'єкт Hall, який потрібно перетворити</param>
    /// <param name="services">Колекція об'єктів Service, які потрібно включити у відповідь</param>
    /// <returns>Об'єкт HallResponse, що містить дані про зал та надані послуги</returns>
    private static HallResponse MapToResponse(
        Hall hall,
        IReadOnlyCollection<Service> services)
    {
        var serviceDictionary = services
            .ToDictionary(s => s.Id);

        return MapToResponse(hall, serviceDictionary);
    }

    /// <summary>
    /// Метод для перетворення об'єкта Hall та словника об'єктів Service у об'єкт HallResponse. Використовує словник для швидкого доступу до послуг за ідентифікатором та створює об'єкт HallResponse з відповідними даними.
    /// </summary>
    /// <param name="hall">Об'єкт Hall, який потрібно перетворити</param>
    /// <param name="services">Словник об'єктів Service для швидкого доступу за ідентифікатором</param>
    /// <returns>Об'єкт HallResponse, що містить дані про зал та надані послуги</returns>
    private static HallResponse MapToResponse(
        Hall hall,
        IReadOnlyDictionary<Guid, Service> services)
    {
        return new HallResponse
        {
            Id = hall.Id,
            Name = hall.Name,
            Capacity = hall.Capacity,
            HourlyRate = hall.HourlyRate,

            Services = hall.Services
                .Select(hallService =>
                {
                    var service = services[hallService.ServiceId];

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
}