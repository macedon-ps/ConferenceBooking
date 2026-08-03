using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Repositories;

/// <summary>
/// Клас репозиторію для роботи з бронюваннями конференцій, реалізує інтерфейс IBookingRepository.
/// </summary>
public class BookingRepository : IBookingRepository
{
    /// <summary>
    /// Контекст бази даних для доступу до таблиці бронювань.
    /// </summary>
    private readonly ConferenceBookingDbContext _context;

    /// <summary>
    /// Конструктор класу BookingRepository, який приймає контекст бази даних як параметр.
    /// </summary>
    /// <param name="context">Контекст бази даних</param>
    public BookingRepository(ConferenceBookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод для отримання бронювання за його унікальним ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання</param>
    /// <returns>Бронювання або null, якщо не знайдено</returns>
    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Include(b => b.Services)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    /// <summary>
    /// Метод для отримання всіх бронювань, пов'язаних з конкретним залом.
    /// </summary>
    /// <param name="hallId">Унікальний ідентифікатор залу</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<Booking>> GetByHallAsync(Guid hallId)
    {
        return await _context.Bookings
            .Include(b => b.Services)
            .Where(b => b.HallId == hallId)
            .ToListAsync();
    }

    /// <summary>
    /// Метод для перевірки наявності конфліктів бронювання для конкретного залу в заданий проміжок часу.
    /// </summary>
    /// <param name="hallId">Унікальний ідентифікатор залу</param>
    /// <param name="startTime">Час початку бронювання</param>
    /// <param name="endTime">Час завершення бронювання</param>
    /// <returns>True, якщо є конфлікт бронювання, інакше False</returns>
    public async Task<bool> HasConflictAsync(
        Guid hallId,
        DateTime startTime,
        DateTime endTime)
    {
        return await _context.Bookings
            .AnyAsync(b =>
                b.HallId == hallId &&
                b.StartTime < endTime &&
                b.EndTime > startTime);
    }

    /// <summary>
    /// Метод для додавання нового бронювання до бази даних.
    /// </summary>
    /// <param name="booking">Бронювання для додавання</param>
    /// <returns></returns>
    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    /// <summary>
    /// Метод для оновлення існуючого бронювання в базі даних.
    /// </summary>
    /// <param name="booking">Бронювання для оновлення</param>
    public void Update(Booking booking)
    {
        _context.Bookings.Update(booking);
    }

    /// <summary>
    /// Метод для видалення бронювання з бази даних.
    /// </summary>
    /// <param name="booking">Бронювання для видалення</param>
    public void Delete(Booking booking)
    {
        _context.Bookings.Remove(booking);
    }
}