using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class ConcertStorage : IConcertStorage
    {
        private readonly StorageFile<Concert> _storageFile;

        public ConcertStorage(string filePath, string tableName)
        {
            _storageFile = new StorageFile<Concert>(filePath, tableName);
        }

        public async Task AddConcertAsync(Concert concert, CancellationToken token)
        {
            await _storageFile.AddAsync(concert, token);
        }

        public async Task DeleteConcertAsync(Concert concert, CancellationToken token)
        {
            // Удаление на основе ID или других уникальных полей концерта
            await _storageFile.DeleteAsync(c => c.Id == concert.Id, token);
        }

        public async Task<IReadOnlyCollection<Concert>> GetAllConcerts(CancellationToken token)
        {
            var concerts = await _storageFile.GetAllAsync(token);
            return concerts.AsReadOnly();
        }
    }
}