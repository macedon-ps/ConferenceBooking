namespace ConferenceBooking.Api.Models;

/// <summary>
/// Стандартна модель відповіді API при виникненні помилки.
/// </summary>
public sealed class ErrorResponse
{
    /// <summary>
    /// HTTP-код відповіді.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// Опис помилки.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    
}
