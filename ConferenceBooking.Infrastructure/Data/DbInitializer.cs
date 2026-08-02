using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        ConferenceBookingDbContext context)
    {
        // Убеждаемся, что все миграции применены.
        await context.Database.MigrateAsync();

        // Если услуги уже существуют, повторно их не создаём.
        if (await context.Services.AnyAsync())
            return;

        // Создаём справочник услуг.
        var projector = Service.Create("Проектор", 500m);
        var wiFi = Service.Create("Wi-Fi", 300m);
        var sound = Service.Create("Звук", 700m);

        context.Services.AddRange(
            projector,
            wiFi,
            sound);

        // Создаём конференц-залы.
        var hallA = Hall.Create(
            "Зал А",
            50,
            2000m);

        var hallB = Hall.Create(
            "Зал B",
            100,
            3500m);

        var hallC = Hall.Create(
            "Зал C",
            30,
            1500m);

        // Назначаем услуги залам.
        hallA.AddService(projector.Id);
        hallA.AddService(wiFi.Id);
        hallA.AddService(sound.Id);

        hallB.AddService(projector.Id);
        hallB.AddService(sound.Id);

        hallC.AddService(wiFi.Id);

        context.Halls.AddRange(
            hallA,
            hallB,
            hallC);

        await context.SaveChangesAsync();
    }
}