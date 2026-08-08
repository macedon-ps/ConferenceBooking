using ConferenceBooking.Api.Middleware;
using ConferenceBooking.Api.Swagger;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;
using ConferenceBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IHallRepository, HallRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHallApplicationService, HallApplicationService>();
builder.Services.AddScoped<IServiceApplicationService, ServiceApplicationService>();
builder.Services.AddScoped<IBookingApplicationService, BookingApplicationService>();
builder.Services.AddScoped<IBookingCostCalculator, BookingCostCalculator>();

builder.Services.AddDbContext<ConferenceBookingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();                      // новий шаблон

builder.Services.AddEndpointsApiExplorer();         // старий формат
builder.Services.AddSwaggerGen(options =>
{
    // Додати підтримку анотацій Swagger для контролерів та моделей
    options.EnableAnnotations();

    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Conference Booking API",
            Version = "v1",
            Description = """
            REST API for managing conference halls and bookings.

            Функції: 
            
            • Управління залами 
            • Управління сервісами 
            • Управління бронюванням 
            • Пошук доступних залів 
            • Автоматичний розрахунок вартості бронювання
            """,

            Contact = new OpenApiContact
            {
                Name = "Conference Booking Project"
            }
        });

    // Додати підтримку прикладів для моделей
    options.OperationFilter<DefaultResponsesOperationFilter>();
    
    // Додати підтримку прикладів для маршрутів
    options.OperationFilter<SwaggerRouteExamplesOperationFilter>();
    
    // Додати підтримку прикладів для моделей
    options.ExampleFilters();
    
    // Додати підтримку прикладів для маршрутів
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // Додати підтримку XML-коментарів для контролерів та моделей
    options.IncludeXmlComments(xmlPath);
    
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

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
    app.UseSwaggerUI();
    /* можливість розширення Swagger UI кастомним JavaScript 
       app.UseSwaggerUI(options =>
        {
            options.InjectJavascript("/swagger/conference-booking-swagger.js");
        });
    */
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();