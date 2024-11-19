using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class MusicianStorage : IMusicianStorage
    {
        private readonly IStorageDataBase<Musician> _storageDataBase;

        public MusicianStorage(string connectionString, string tableName)
        {
            _storageDataBase = new StorageDataBase<Musician>(connectionString, tableName);
        }
        public MusicianStorage(IStorageDataBase<Musician> storageDataBase)
        {
            _storageDataBase = storageDataBase;
        }

        public async Task AddMusicianAsync(Musician musician, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _storageDataBase.AddAsync(musician, token);
        }

        public async Task DeleteMusicianAsync(Musician musician, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            // Удаление на основе уникального идентификатора или других уникальных полей музыканта
            await _storageDataBase.DeleteAsync("id = @Id", new { Id = musician.Id }, token);
        }

        public async Task<IReadOnlyCollection<Musician>> GetAllMusiciansAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _storageDataBase.GetListAsync("", null, token); // Получаем все записи
        }
    }
}