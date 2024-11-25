using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class SoundOnConcertPresenter : ISoundOnConcertPresenter
    {
        private readonly IStorageDataBase<SoundOnConcert> _soundOnConcertStorage;

        public SoundOnConcertPresenter(IStorageDataBase<SoundOnConcert> soundOnConcertStorage)
        {
            _soundOnConcertStorage = soundOnConcertStorage;
        }

        public SoundOnConcertPresenter(ApplicationDbContext dbContext)
        {
            _soundOnConcertStorage = new StorageDataBase<SoundOnConcert>(dbContext);
        }

        public SoundOnConcertPresenter()
        {}

        // Добавление связи между концертом и звуком
        public async Task AddSoundOnConcertAsync(Guid concertId, Guid soundId, CancellationToken token)
        {
            var soundOnConcert = new SoundOnConcert(concertId, soundId);
            await _soundOnConcertStorage.AddAsync(soundOnConcert, token);
        }

        // Получение всех записей о связях между концертами и звуками
        public async Task<IReadOnlyCollection<SoundOnConcert>> GetSoundOnConcertAsync(CancellationToken token)
        {
            return await _soundOnConcertStorage.GetListAsync(null, null, token);
        }
    }
}