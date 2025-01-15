using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Presenter
{
    public class SoundOnConcertPresenter : ISoundOnConcertPresenter
    {
        private readonly IStorageDataBase<SoundOnConcert> _soundOnConcertStorage;

        public SoundOnConcertPresenter(IStorageDataBase<SoundOnConcert> soundOnConcertStorage)
        {
            _soundOnConcertStorage = soundOnConcertStorage ?? throw new ArgumentNullException(nameof(soundOnConcertStorage));
        }

        public SoundOnConcertPresenter(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _soundOnConcertStorage = new StorageDataBase<SoundOnConcert>(dbContext);
        }

        public SoundOnConcertPresenter() {}

        // Добавление связи между концертом и звуком
        public async Task AddSoundOnConcertAsync(Guid concertId, Guid soundId, CancellationToken token)
        {
            if (concertId == Guid.Empty)
                throw new ArgumentException("Concert ID cannot be empty.", nameof(concertId));
            if (soundId == Guid.Empty)
                throw new ArgumentException("Sound ID cannot be empty.", nameof(soundId));

            var soundOnConcert = new SoundOnConcert(concertId, soundId);
            await _soundOnConcertStorage.AddAsync(soundOnConcert, token);
        }

        // Получение всех записей о связях между концертами и звуками
        public async Task<IReadOnlyCollection<SoundOnConcert>> GetSoundOnConcertAsync(CancellationToken token)
        {
            return await _soundOnConcertStorage.GetListAsync(query => query, token);
        }

        // Удаление связи между концертом и звуком
        public async Task DeleteSoundOnConcertAsync(Guid concertId, Guid soundId, CancellationToken token)
        {
            await _soundOnConcertStorage.DeleteAsync(
                query => query.Where(s => s.ConcertId == concertId && s.SoundId == soundId),
                token
            );
        }
    }
}
