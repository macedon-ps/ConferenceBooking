using ConferenceBooking.Domain.Interfaces;

namespace ConferenceBooking.Infrastructure.Data
{
    /// <summary>
    /// Клас UnitOfWork реалізує інтерфейс IUnitOfWork і відповідає за управління транзакціями та збереження змін у базі даних.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Контекст бази даних для доступу до таблиць та управління транзакціями.
        /// </summary>
        private readonly ConferenceBookingDbContext _context;

        /// <summary>
        /// Конструктор класу UnitOfWork, який приймає контекст бази даних як параметр.
        /// </summary>
        /// <param name="context">Контекст бази даних</param>
        public UnitOfWork(ConferenceBookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод для збереження змін у базі даних. Повертає кількість змінених записів у базі даних після виконання операцій збереження.
        /// </summary>
        /// <returns>Кількість змінених записів у базі даних</returns>
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
