using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class SoundPresenter : IMusicPresenter
    {
        private readonly IStorageDataBase<Sound> _soundStorage;

        // Конструктор с передачей репозитория
        public SoundPresenter(IStorageDataBase<Sound> soundStorage)
        {
            _soundStorage = soundStorage;
        }

        // Конструктор по умолчанию с созданием репозитория
        public SoundPresenter(ApplicationDbContext dbContext)
        {
            _soundStorage = new StorageDataBase<Sound>(dbContext);
        }

        // Добавление музыки
        public async Task<Sound> AddMusicAsync(string name, string author, CancellationToken token)
        {
            var id = Guid.NewGuid();
            var sound = new Sound(id, name, author);
            await _soundStorage.AddAsync(sound, token);
            return sound; // Возвращаем добавленное произведение
        }

        // Удаление музыки
        public async Task DeleteMusicAsync(Sound sound, CancellationToken token)
        {
            await _soundStorage.DeleteAsync(null, new { sound.Id }, token);
        }

        // Получение всех произведений музыки
        public async Task<IReadOnlyCollection<Sound>> GetMusicAsync(CancellationToken token)
        {
            return await _soundStorage.GetListAsync(null, null, token);
        }
    }
}