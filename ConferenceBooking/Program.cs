using ConferenceBooking.Api.Middleware;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;
using ConferenceBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IHallRepository, HallRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHallApplicationService, HallApplicationService>();
builder.Services.AddScoped<IServiceApplicationService, ServiceApplicationService>();
builder.Services.AddScoped<IBookingApplicationService, BookingApplicationService>();

builder.Services.AddDbContext<ConferenceBookingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();                      // новий шаблон

builder.Services.AddEndpointsApiExplorer();         // старий формат
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Conference Booking API",
            Version = "v1",
            Description = """
            REST API for managing conference halls and bookings.

            Features:

            • Hall management
            • Booking management
            • Search for available halls
            • Automatic booking cost calculation
            """,

            Contact = new OpenApiContact
            {
                Name = "Conference Booking Project"
            }
        });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

/* Ініціалізація бази даних первинними даними, якщо вона порожня 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ConferenceBookingDbContext>();

    await DbInitializer.InitializeAsync(context);
}*/

if (app.Environment.IsDevelopment())
{
    // вбудований OpenAPI
    app.MapOpenApi();

    // Swagger UI
    app.UseSwagger();
    // використання статичних файлів для Swagger UI
    app.UseStaticFiles();
    app.UseSwaggerUI(options =>
    {
        options.InjectJavascript(
            "/swagger/conference-booking-swagger.js");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();