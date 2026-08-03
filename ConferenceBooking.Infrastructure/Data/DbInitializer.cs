using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data;

/// <summary>
/// Клас для ініціалізації бази даних з початковими даними, включаючи створення залів та послуг.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Метод для ініціалізації бази даних з початковими даними, включаючи створення залів та послуг.
    /// </summary>
    /// <param name="context">Контекст бази даних</param>
    /// <returns></returns>
    public static async Task InitializeAsync(
        ConferenceBookingDbContext context)
    {
        // Упевняємося, що база даних створена та застосовуємо міграції
        await context.Database.MigrateAsync();

        // Якщо послуги вже існують, повторно їх не створюємо
        if (await context.Services.AnyAsync())
            return;

        // Створюємо довідник послуг.
        var projector = Service.Create("Проектор", 500m);
        var wiFi = Service.Create("Wi-Fi", 300m);
        var sound = Service.Create("Звук", 700m);

        context.Services.AddRange(
            projector,
            wiFi,
            sound);

        // Створюємо конференц-зали.
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

        // Призначаємо послуги залам.
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