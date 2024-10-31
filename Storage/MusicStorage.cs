using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class MusicStorage : IMusicStorage
    {
        private readonly StorageFile<Music> _storageFile;

        public MusicStorage(string filePath, string tableName)
        {
            _storageFile = new StorageFile<Music>(filePath, tableName);
        }

        public async Task AddMusicAsync(Music music, CancellationToken token)
        {
            await _storageFile.AddAsync(music, token);
        }

        public async Task DeleteMusicAsync(Music music, CancellationToken token)
        {
            // Удаление на основе уникального идентификатора или другого уникального поля записи
            await _storageFile.DeleteAsync(m => m.Id == music.Id, token);
        }

        public async Task<IReadOnlyCollection<Music>> GetAllMusicAsync(CancellationToken token)
        {
            var musicList = await _storageFile.GetAllAsync(token);
            return musicList.AsReadOnly();
        }
    }
}