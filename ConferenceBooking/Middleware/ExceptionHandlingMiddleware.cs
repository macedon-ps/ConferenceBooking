using ConferenceBooking.Domain.Exceptions;
using System.Diagnostics;
using System.Text.Json;

namespace ConferenceBooking.Api.Middleware;

/// <summary>
/// Клас проміжного програмного забезпечення для обробки винятків у веб-додатку ASP.NET Core. Він перехоплює необроблені винятки, логірує їх та повертає відповідь з відповідним кодом стану HTTP та повідомленням про помилку.
/// </summary>
public class ExceptionHandlingMiddleware
{
    /// <summary>
    /// Наступний делегат запиту, який представляє наступний компонент у конвеєрі обробки запитів. Використовується для передачі запиту далі після обробки винятків.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Логер для запису повідомлень про помилки та винятки. Використовується для логування необроблених винятків, що виникають під час обробки запитів.
    /// </summary>
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Конструктор класу ExceptionHandlingMiddleware, який приймає делегат запиту та логер як параметри. Ініціалізує приватні поля для обробки запитів та логування винятків.
    /// </summary>
    /// <param name="next">Делегат запиту, який представляє наступний компонент у конвеєрі обробки запитів.</param>
    /// <param name="logger">Логер для запису повідомлень про помилки та винятки.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Метод InvokeAsync обробляє HTTP-запити та перехоплює необроблені винятки. Якщо виникає виняток, він логірує його та викликає метод HandleExceptionAsync для формування відповіді з відповідним кодом стану HTTP та повідомленням про помилку.
    /// </summary>
    /// <param name="context">Об'єкт HttpContext, який містить інформацію про поточний HTTP-запит та відповідь.</param>
    /// <returns>Завдання, яке представляє асинхронну операцію обробки запиту.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms.",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            _logger.LogError(
                exception,
                "Unhandled exception during HTTP {Method} {Path} after {ElapsedMilliseconds} ms.",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);

            await HandleExceptionAsync(
                context,
                exception);
        }
    }

    /// <summary>
    /// Метод HandleExceptionAsync формує відповідь на HTTP-запит у разі виникнення винятку. Він визначає відповідний код стану HTTP на основі типу винятку, формує повідомлення про помилку та повертає JSON-відповідь клієнту.
    /// </summary>
    /// <param name="context">Об'єкт HttpContext, який містить інформацію про поточний HTTP-запит та відповідь.</param>
    /// <param name="exception">Виняток, який виник під час обробки запиту.</param>
    /// <returns>Завдання, яке представляє асинхронну операцію формування відповіді на виняток.</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            KeyNotFoundException =>
                StatusCodes.Status404NotFound,

            InvalidOperationException =>
                StatusCodes.Status409Conflict,

            DomainException =>
                StatusCodes.Status400BadRequest,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        var message = statusCode ==
                      StatusCodes.Status500InternalServerError
            ? "An unexpected error occurred."
            : exception.Message;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            statusCode,
            message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}