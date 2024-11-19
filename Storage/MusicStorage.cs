using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class MusicStorage : IMusicStorage
    {
        private readonly StorageDataBase<Music> _storageDataBase;

        public MusicStorage(string connectionString, string tableName)
        {
            _storageDataBase = new StorageDataBase<Music>(connectionString, tableName);
        }

        public async Task AddMusicAsync(Music music, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _storageDataBase.AddAsync(music, token);
        }

        public async Task DeleteMusicAsync(Music music, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            // Удаление на основе уникального идентификатора или другого уникального поля записи
            await _storageDataBase.DeleteAsync("id = @Id", new { Id = music.Id }, token);
        }

        public async Task<IReadOnlyCollection<Music>> GetAllMusicAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _storageDataBase.GetListAsync("", null, token); // Получаем все записи
        }
    }
}