using ConferenceBooking.Domain.Models.Reports;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Repositories;

/// <summary>
/// Клас репозиторію для отримання аналітичних даних по бронюванням.
/// </summary>
public class ReportRepository : IReportRepository
{
    private readonly ConferenceBookingDbContext _context;

    /// <summary>
    /// Ініціалізує репозиторій звітів.
    /// </summary>
    /// <param name="context">Контекст бази даних.</param>
    public ReportRepository(ConferenceBookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Повертає зведену статистику по бронюванням за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Зведена статистика по бронюванням.</returns>
    public async Task<BookingSummaryModel> GetBookingSummaryAsync(DateTime from, DateTime to)
    {
        var bookings = _context.Bookings
            .AsNoTracking()
            .Where(b =>
                b.StartTime < to &&
                b.EndTime > from);

        var totalBookings = await bookings.CountAsync();

        var totalBookedHours = await bookings
            .SumAsync(b =>
                (decimal)EF.Functions.DateDiffMinute(
                    b.StartTime,
                    b.EndTime) / 60m);

        var totalRevenue = await bookings
            .SumAsync(b => b.TotalCost);

        var averageBookingCost = totalBookings > 0
            ? await bookings.AverageAsync(b => b.TotalCost)
            : 0m;

        return new BookingSummaryModel
        {
            TotalBookings = totalBookings,
            TotalBookedHours = totalBookedHours,
            TotalRevenue = totalRevenue,
            AverageBookingCost = averageBookingCost
        };
    }

    /// <summary>
    /// Повертає статистику використання конференц-залів за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика використання конференц-залів.</returns>
    public async Task<IReadOnlyCollection<HallUtilizationModel>>
    GetHallUtilizationAsync(DateTime from, DateTime to)
    {
        var result = await _context.Bookings
            .AsNoTracking()
            .Where(b =>
                b.StartTime < to &&
                b.EndTime > from)
            .Join(
                _context.Halls.AsNoTracking(),
                booking => booking.HallId,
                hall => hall.Id,
                (booking, hall) => new
                {
                    Booking = booking,
                    Hall = hall
                })
            .GroupBy(x => new
            {
                x.Hall.Id,
                x.Hall.Name
            })
            .Select(group => new HallUtilizationModel
            {
                HallId = group.Key.Id,
                HallName = group.Key.Name,
                BookingCount = group.Count(),
                TotalBookedHours = group.Sum(x =>
                    (decimal)EF.Functions.DateDiffMinute(
                        x.Booking.StartTime,
                        x.Booking.EndTime) / 60m),
                TotalRevenue = group.Sum(x => x.Booking.TotalCost)
            })
            .OrderByDescending(x => x.TotalBookedHours)
            .ToListAsync();

        return result;
    }

    /// <summary>
    /// Повертає статистику використання послуг за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Статистика використання послуг.</returns>
    public async Task<IReadOnlyCollection<PopularServiceModel>>
    GetPopularServicesAsync(
        DateTime from,
        DateTime to)
    {
        var result = await _context.BookingServices
            .AsNoTracking()
            .Join(
                _context.Bookings.AsNoTracking(),
                bookingService => bookingService.BookingId,
                booking => booking.Id,
                (bookingService, booking) => new
                {
                    BookingService = bookingService,
                    Booking = booking
                })
            .Where(x =>
                x.Booking.StartTime < to &&
                x.Booking.EndTime > from)
            .Join(
                _context.Services.AsNoTracking(),
                x => x.BookingService.ServiceId,
                service => service.Id,
                (x, service) => new
                {
                    BookingService = x.BookingService,
                    Service = service
                })
            .GroupBy(x => new
            {
                x.Service.Id,
                x.Service.Name
            })
            .Select(group => new PopularServiceModel
            {
                ServiceId = group.Key.Id,
                ServiceName = group.Key.Name,
                UsageCount = group.Count()
            })
            .OrderByDescending(x => x.UsageCount)
            .ToListAsync();

        return result;
    }
}