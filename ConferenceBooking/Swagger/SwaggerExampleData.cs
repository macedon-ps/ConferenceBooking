namespace ConferenceBooking.Api.Swagger;

/// <summary>
/// Клас, що містить прикладні дані для Swagger документації. Використовується для надання конкретних прикладів значень UUID та дат, які можуть бути використані у запитах до API.
/// </summary>
public static class SwaggerExampleData
{
    /// <summary>
    /// Guid Id залу A.
    /// </summary>
    public static readonly Guid HallId_A = Guid.Parse("34645abc-2f6d-41e2-af41-c1e4f3142c44");

    /// <summary>
    /// Guid Id залу B.
    /// </summary>
    public static readonly Guid HallId_B = Guid.Parse("1f785d2d-0ade-42ac-b1e0-7aa7e5e24a74");

    /// <summary>
    /// Guid Id залу C.
    /// </summary>
    public static readonly Guid HallId_C = Guid.Parse("9df6ada8-bf4c-4527-9909-597407adbcb3");
    
    /// <summary>
    /// Guid Id залу D.
    /// </summary>
    public static readonly Guid HallId_D = Guid.Parse("6f555342-7f57-4037-848f-d8dab4196e0a");

    /// <summary>
    /// Guid Id залу E.
    /// </summary>
    public static readonly Guid HallId_E = Guid.Parse("5abd1282-7e56-42d4-8838-e2e23151ca00");

    /// <summary>
    /// Guid Id залу F.
    /// </summary>
    public static readonly Guid HallId_F = Guid.Parse("498948b4-15c6-4a6e-ad64-c7c7292f844e");

    /// <summary>
    /// Guid Id послуги Sound.
    /// </summary>
    public static readonly Guid SoundId = Guid.Parse("430ad241-7be1-4fc3-a2e8-16ec281026b9");

    /// <summary>
    /// Guid Id послуги Projector.
    /// </summary>
    public static readonly Guid ProjectorId = Guid.Parse("0ab0ae22-6c6d-46b6-bfe1-773fa7fde3ab");

    /// <summary>
    /// Guid Id послуги Wifi.
    /// </summary>
    public static readonly Guid WifiId = Guid.Parse("16a4f62f-ca2b-4eef-be95-9c8824ca672c");
    
    /// <summary>
    /// Guid Id бронювання для зали C.
    /// </summary>
    public static readonly Guid BookingId_C = Guid.Parse("63a8c1f6-8dff-4b2f-b5d3-b5702bbb4a03");

    /// <summary>
    /// Guid Id бронювання для зали C (друге бронювання).
    /// </summary>
    public static readonly Guid BookingId_C2 = Guid.Parse("b0128bf0-e5cf-4fdc-9a12-6eb3b20adeaf");

    /// <summary>
    /// Guid Id бронювання для зали F.
    /// </summary>
    public static readonly Guid BookingId_F = Guid.Parse("3999981e-0638-4087-937c-c280b1495ffa");

    /// <summary>
    /// Початковий час бронювання.
    /// </summary>
    public static readonly DateTime StartTime = new(2026, 8, 8, 10, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Кінцевий час бронювання.
    /// </summary>
    public static readonly DateTime EndTime = new(2026, 8, 8, 18, 0, 0, DateTimeKind.Utc);


    /// <summary>
    /// Початковий час другого бронювання.
    /// </summary>
    public static readonly DateTime StartTime2 = new(2026, 8, 10, 14, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Кінцевий час другого бронювання.
    /// </summary>
    public static readonly DateTime EndTime2 = new(2026, 8, 10, 16, 0, 0, DateTimeKind.Utc);

    public static readonly DateTime ReportFrom = new(2026, 8, 1, 0, 0, 0);

    public static readonly DateTime ReportTo = new(2026, 8, 10, 23, 59, 59);

    /// <summary>
    /// Місткість залу.
    /// </summary>
    public static readonly int Capacity = 100;

    /// <summary>
    /// Місткість другого залу.
    /// </summary>
    public static readonly int Capacity2 = 150;

}   