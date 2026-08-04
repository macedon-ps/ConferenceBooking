using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Repositories;

/// <summary>
/// Клас репозиторію для роботи з послугами конференцій, реалізує інтерфейс IServiceRepository.
/// </summary>
public class ServiceRepository : IServiceRepository
{
    /// <summary>
    /// Контекст бази даних для доступу до таблиці послуг.
    /// </summary>
    private readonly ConferenceBookingDbContext _context;

    /// <summary>
    /// Конмтруктор класу ServiceRepository, який приймає контекст бази даних як параметр.
    /// </summary>
    /// <param name="context">Контекст бази даних</param>
    public ServiceRepository(ConferenceBookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод для отримання послуги за її унікальним ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор послуги</param>
    /// <returns>Послуга або null, якщо не знайдено</returns>
    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Services
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Метод для отримання всіх послуг з бази даних.
    /// </summary>
    /// <returns>Список всіх послуг</returns>
    public async Task<IReadOnlyList<Service>> GetAllAsync()
    {
        return await _context.Services
            .ToListAsync();
    }

    /// <summary>
    /// Метод для додавання нової послуги до бази даних.
    /// </summary>
    /// <param name="service">Послуга для додавання</param>
    /// <returns></returns>
    public async Task AddAsync(Service service)
    {
        await _context.Services.AddAsync(service);
    }

    /// <summary>
    /// Метод для оновлення існуючої послуги в базі даних.
    /// </summary>
    /// <param name="service">Послуга для оновлення</param>
    public void Update(Service service)
    {
        _context.Services.Update(service);
    }
    
    /// <summary>
    /// Метод для видалення послуги з бази даних.
    /// </summary>
    /// <param name="service">Послуга для видалення</param> 
    public void Delete(Service service)
    {
        _context.Services.Remove(service);
    }

    /// <summary>
    /// Метод для перевірки існування послуги за її унікальним ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор послуги</param>
    /// <returns>True, якщо послуга існує, інакше False</returns>
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Services
            .AnyAsync(s => s.Id == id);
    }

    /// <summary>
    /// Метод для отримання списку послуг за їх унікальними ідентифікаторами.
    /// </summary>
    /// <param name="ids">Колекція унікальних ідентифікаторів послуг</param>
    /// <returns>Список послуг, що відповідають заданим ідентифікаторам</returns>
    public async Task<IReadOnlyList<Service>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var serviceIds = ids
        .Distinct()
        .ToList();

        if (serviceIds.Count == 0)
        {
            return Array.Empty<Service>();
        }

        return await _context.Services
            .Where(s => serviceIds.Contains(s.Id))
            .ToListAsync();
    }
}