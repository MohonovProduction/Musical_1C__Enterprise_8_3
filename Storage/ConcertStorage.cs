using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class ConcertStorage : IConcertStorage
    {
        private readonly StorageDataBase<Concert> _storageDataBase;

        public ConcertStorage(string connectionString, string tableName)
        {
            _storageDataBase = new StorageDataBase<Concert>(connectionString, tableName);
        }

        public async Task AddConcertAsync(Concert concert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _storageDataBase.AddAsync(concert, token);
        }

        public async Task DeleteConcertAsync(Concert concert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            // Удаление на основе ID или других уникальных полей концерта
            await _storageDataBase.DeleteAsync("id = @Id", new { Id = concert.Id }, token);
        }

        public async Task<IReadOnlyCollection<Concert>> GetAllConcertsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _storageDataBase.GetListAsync("", null, token); // Получаем все записи
        }
    }
}