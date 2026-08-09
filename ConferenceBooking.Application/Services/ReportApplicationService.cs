using ConferenceBooking.Application.DTOs.Reports;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Application.Services;

/// <summary>
/// Клас надає функціонал для формування звітів та аналітики по бронюванням.
/// </summary>
public class ReportApplicationService : IReportApplicationService
{
    private readonly IReportRepository _reportRepository;

    /// <summary>
    /// Конструктор класу ініціалізує сервіс звітів.
    /// </summary>
    /// <param name="reportRepository">Репозиторій звітів.</param>
    public ReportApplicationService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    /// <summary>
    /// Метод повернення зведеної статистики з бронювань. 
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    public async Task<BookingSummaryResponse> GetBookingSummaryAsync(DateTime from,   DateTime to)
    {
        ValidatePeriod(from, to);

        var model = await _reportRepository
            .GetBookingSummaryAsync(from, to);

        return new BookingSummaryResponse
        {
            TotalBookings = model.TotalBookings,
            TotalBookedHours = model.TotalBookedHours,
            TotalRevenue = model.TotalRevenue,
            AverageBookingCost = model.AverageBookingCost
        };
    }

    /// <summary>
    /// Метод повернення статистики завантажень залів за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    public async Task<IReadOnlyCollection<HallUtilizationResponse>>
        GetHallUtilizationAsync(DateTime from, DateTime to)
    {
        ValidatePeriod(from, to);

        var models = await _reportRepository
            .GetHallUtilizationAsync(from, to);

        return models
            .Select(model => new HallUtilizationResponse
            {
                HallId = model.HallId,
                HallName = model.HallName,
                BookingCount = model.BookingCount,
                TotalBookedHours = model.TotalBookedHours,
                TotalRevenue = model.TotalRevenue
            })
            .ToList();
    }

    /// <summary>
    /// Метод повертає статистику використання послуг за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    public async Task<IReadOnlyCollection<PopularServiceResponse>>
        GetPopularServicesAsync(DateTime from, DateTime to)
    {
        ValidatePeriod(from, to);

        var models = await _reportRepository
            .GetPopularServicesAsync(from, to);

        return models
            .Select(model => new PopularServiceResponse
            {
                ServiceId = model.ServiceId,
                ServiceName = model.ServiceName,
                UsageCount = model.UsageCount
            })
            .ToList();
    }

    private static void ValidatePeriod(DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new DomainException(
            "The 'from' date must be earlier than the 'to' date.");
        }
    }
}