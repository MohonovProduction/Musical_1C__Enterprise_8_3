using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class ConcertStorage : IConcertStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public ConcertStorage(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление концерта
        public async Task AddConcertAsync(Concert concert, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                await dbContext.Set<Concert>().AddAsync(concert, token);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Удаление концерта
        public async Task DeleteConcertAsync(Concert concert, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                dbContext.Set<Concert>().Remove(concert);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Получение всех концертов
        public async Task<IReadOnlyCollection<Concert>> GetAllConcertsAsync(CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.Set<Concert>().ToListAsync(token);
            }
        }

        // Получение концертов с фильтрацией по имени
        public async Task<IReadOnlyCollection<Concert>> GetConcertsByNameAsync(string name, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.Set<Concert>()
                    .Where(c => c.Name.Contains(name))
                    .ToListAsync(token);
            }
        }

        // Получение концертов по типу
        public async Task<IReadOnlyCollection<Concert>> GetConcertsByTypeAsync(string type, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.Set<Concert>()
                    .Where(c => c.Type.Contains(type))
                    .ToListAsync(token);
            }
        }

    }
}