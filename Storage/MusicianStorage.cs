using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class MusicianStorage : IMusicianStorage
    {
        private readonly StorageFile<Musician> _storageFile;

        public MusicianStorage(string filePath, string tableName)
        {
            _storageFile = new StorageFile<Musician>(filePath, tableName);
        }

        public async Task AddMusicianAsync(Musician musician, CancellationToken token)
        {
            await _storageFile.AddAsync(musician, token);
        }

        public async Task DeleteMusicianAsync(Musician musician, CancellationToken token)
        {
            // Удаление на основе уникального идентификатора или других уникальных полей музыканта
            await _storageFile.DeleteAsync(m => m.Id == musician.Id, token);
        }

        public async Task<IReadOnlyCollection<Musician>> GetAllMusiciansAsync(CancellationToken token)
        {
            var musicians = await _storageFile.GetAllAsync(token);
            return musicians.AsReadOnly();
        }
    }
}