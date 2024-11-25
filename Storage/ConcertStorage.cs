using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class ConcertStorage : IConcertStorage
    {
        private readonly DbContext _dbContext;

        public ConcertStorage(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление концерта
        public async Task AddConcertAsync(Concert concert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _dbContext.Set<Concert>().AddAsync(concert, token);
            await _dbContext.SaveChangesAsync(token);
        }

        // Удаление концерта
        public async Task DeleteConcertAsync(Concert concert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            _dbContext.Set<Concert>().Remove(concert);
            await _dbContext.SaveChangesAsync(token);
        }

        // Получение всех концертов
        public async Task<IReadOnlyCollection<Concert>> GetAllConcertsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.Set<Concert>().ToListAsync(token);
        }

        // Получение концертов с фильтрацией по имени (например)
        public async Task<IReadOnlyCollection<Concert>> GetConcertsByNameAsync(string name, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.Set<Concert>()
                .Where(c => c.Name.Contains(name))
                .ToListAsync(token);
        }

        // Получение концертов по типу (например)
        public async Task<IReadOnlyCollection<Concert>> GetConcertsByTypeAsync(string type, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.Set<Concert>()
                .Where(c => c.Type.Contains(type))
                .ToListAsync(token);
        }
    }
}
