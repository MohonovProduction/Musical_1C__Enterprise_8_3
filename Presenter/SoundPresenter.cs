using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Presenter
{
    public class SoundPresenter : ISoundPresenter
    {
        private readonly IStorageDataBase<Sound> _soundStorage;

        // Конструктор, принимающий IStorageDataBase<Sound>
        public SoundPresenter(IStorageDataBase<Sound> soundStorage)
        {
            _soundStorage = soundStorage;
        }

        // Конструктор, принимающий ApplicationDbContext
        public SoundPresenter(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            using (var ctx = dbContext.CreateDbContext())
            {
                _soundStorage = new StorageDataBase<Sound>(ctx);
            }
        }

        // Добавление произведения музыки
        public async Task<Sound> AddMusicAsync(string name, string author, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be null or whitespace.", nameof(author));

            var sound = new Sound(Guid.NewGuid(), name, author);
            await _soundStorage.AddAsync(sound, token);
            return sound;
        }

        // Удаление произведения музыки
        public async Task DeleteMusicAsync(Guid soundId, CancellationToken token)
        {
            if (soundId == Guid.Empty)
                throw new ArgumentException("Sound ID cannot be empty.", nameof(soundId));

            await _soundStorage.DeleteAsync(query => query.Where(s => s.Id == soundId), token);
        }

        // Получение всех произведений музыки
        public async Task<IReadOnlyCollection<Sound>> GetMusicAsync(CancellationToken token)
        {
            return await _soundStorage.GetListAsync(query => query, token);
        }

        // Получение произведения музыки по ID
        public async Task<Sound> GetMusicByIdAsync(Guid soundId, CancellationToken token)
        {
            if (soundId == Guid.Empty)
                throw new ArgumentException("Sound ID cannot be empty.", nameof(soundId));

            return await _soundStorage.GetSingleAsync(query => query.Where(s => s.Id == soundId), token);
        }
    }
}
