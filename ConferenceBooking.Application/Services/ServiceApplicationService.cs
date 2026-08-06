using ConferenceBooking.Application.DTOs.Services;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConferenceBooking.Application.Services;

/// <summary>
/// Клас сервісу для управління послугами конференцій. Реалізує інтерфейс IServiceApplicationService та надає методи для отримання всіх послуг та отримання конкретної послуги за її унікальним ідентифікатором.
/// </summary>
public class ServiceApplicationService : IServiceApplicationService
{
    /// <summary>
    /// Репозиторій для доступу до даних про послуги конференцій. Використовується для отримання інформації про всі послуги та конкретну послугу за її унікальним ідентифікатором.
    /// </summary>
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Логгер для ведення журналу подій та повідомлень про помилки. Використовується для запису інформації про процеси отримання послуг та обробки помилок.
    /// </summary>
    private readonly ILogger<ServiceApplicationService> _logger;

    /// <summary>
    /// Конструктор класу ServiceApplicationService, який приймає репозиторій для доступу до даних про послуги конференцій та логгер як параметри. Ініціалізує приватні поля для доступу до методів репозиторію та логування.
    /// </summary>
    /// <param name="serviceRepository">Репозиторій для доступу до даних про послуги конференцій.</param>
    /// <param name="logger">Логгер для ведення журналу подій та повідомлень про помилки.</param>
    public ServiceApplicationService(IServiceRepository serviceRepository, ILogger<ServiceApplicationService> logger)
    {
        _serviceRepository = serviceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Метод для отримання всіх послуг конференцій. Використовує репозиторій для отримання списку всіх послуг та перетворює їх у колекцію об'єктів ServiceResponse, яка містить інформацію про кожну послугу (ідентифікатор, назву та ціну).
    /// </summary>
    /// <returns>Колекція об'єктів ServiceResponse з інформацією про всі послуги.</returns>
    public async Task<IReadOnlyCollection<ServiceResponse>> GetAllAsync()
    {
        var services = await _serviceRepository.GetAllAsync();

        return services
            .Select(service => new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Price = service.Price
            })
            .ToList();
    }

    /// <summary>
    /// Метод для отримання конкретної послуги конференції за її унікальним ідентифікатором. Використовує репозиторій для отримання послуги за вказаним ідентифікатором. Якщо послуга не знайдена, генерує виняток KeyNotFoundException та записує попередження у журнал.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор послуги.</param>
    /// <returns>Об'єкт ServiceResponse з інформацією про послугу.</returns>
    /// <exception cref="KeyNotFoundException">Викидається, якщо послуга з вказаним ідентифікатором не знайдена.</exception>
    public async Task<ServiceResponse> GetByIdAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);

        if (service is null)
        {
            _logger.LogWarning("Service retrieval failed. Service not found. ServiceId: {ServiceId}.", id);

            throw new KeyNotFoundException($"Service with ID '{id}' was not found.");
        }

        return new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price
        };
    }
}