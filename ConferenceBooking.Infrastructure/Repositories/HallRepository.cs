using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Repositories;

/// <summary>
/// Клас репозиторію для роботи з залами конференцій, реалізує інтерфейс IHallRepository.
/// </summary>
public class HallRepository : IHallRepository
{
    /// <summary>
    /// Контекст бази даних для доступу до таблиць залів та бронювань.
    /// </summary>
    private readonly ConferenceBookingDbContext _context;

    /// <summary>
    /// Конструктор класу HallRepository, який приймає контекст бази даних як параметр.
    /// </summary>
    /// <param name="context">Контекст бази даних</param>
    public HallRepository(ConferenceBookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод для отримання залу за його унікальним ідентифікатором, включаючи пов'язані сервіси.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу</param>
    /// <returns>Зал або null, якщо не знайдено</returns>
    public async Task<Hall?> GetByIdAsync(Guid id)
    {
        return await _context.Halls
            .Include(h => h.Services)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    /// <summary>
    /// Метод для отримання всіх залів, включаючи пов'язані сервіси.
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<Hall>> GetAllAsync()
    {
        return await _context.Halls
            .Include(h => h.Services)
            .ToListAsync();
    }

    /// <summary>
    /// Метод для отримання доступних залів на основі заданого часу та місткості, включаючи пов'язані сервіси.
    /// </summary>
    /// <param name="startTime">Час початку бронювання</param>
    /// <param name="endTime">Час завершення бронювання</param>
    /// <param name="capacity">Місткість залу</param>
    /// <returns>Список доступних залів</returns>
    public async Task<IReadOnlyList<Hall>> GetAvailableAsync(
        DateTime startTime,
        DateTime endTime,
        int capacity)
    {
        return await _context.Halls
            .Include(h => h.Services)
            .Where(h => h.Capacity >= capacity)
            .Where(h => !_context.Bookings.Any(b =>
                b.HallId == h.Id &&
                b.StartTime < endTime &&
                b.EndTime > startTime))
            .ToListAsync();
    }

    /// <summary>
    /// Метод для додавання нового залу до бази даних.
    /// </summary>
    /// <param name="hall">Зал для додавання</param>
    /// <returns></returns>
    public async Task AddAsync(Hall hall)
    {
        await _context.Halls.AddAsync(hall);
    }

    /// <summary>
    /// Метод для оновлення інформації про існуючий зал у базі даних.
    /// </summary>
    /// <param name="hall">Зал для оновлення</param>
    public void Update(Hall hall)
    {
        _context.Halls.Update(hall);
    }

    /// <summary>
    /// Метод для видалення залу з бази даних.
    /// </summary>
    /// <param name="hall">Зал для видалення</param>
    public void Delete(Hall hall)
    {
        _context.Halls.Remove(hall);
    }

    /// <summary>
    /// Метод для перевірки існування залу за його унікальним ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу</param>
    /// <returns>True, якщо зал існує, інакше False</returns>
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Halls
            .AnyAsync(h => h.Id == id);
    }
}