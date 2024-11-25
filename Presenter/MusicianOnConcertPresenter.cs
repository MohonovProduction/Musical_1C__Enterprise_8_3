using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class MusicianOnConcertPresenter
    {
        private readonly IStorageDataBase<MusicianOnConcert> _musicianOnConcertStorage;

        // Конструктор, принимающий IStorageDataBase<MusicianOnConcert>
        public MusicianOnConcertPresenter(IStorageDataBase<MusicianOnConcert> musicianOnConcertStorage)
        {
            _musicianOnConcertStorage = musicianOnConcertStorage;
        }

        // Конструктор по умолчанию, использующий MusicianOnConcertStorage
        public MusicianOnConcertPresenter(ApplicationDbContext dbContext)
        {
            _musicianOnConcertStorage = new StorageDataBase<MusicianOnConcert>(dbContext);
        }

        public MusicianOnConcertPresenter()
        {
            
        }

        // Добавление музыканта на концерт
        public async Task AddMusicianOnConcertAsync(Guid concertId, Guid musicianId, CancellationToken token)
        {
            var musicianOnConcert = new MusicianOnConcert(concertId, musicianId);
            await _musicianOnConcertStorage.AddAsync(musicianOnConcert, token);
        }

        // Получение всех музыкантов на концертах
        public async Task<IReadOnlyCollection<MusicianOnConcert>> GetMusicianOnConcertAsync(CancellationToken token)
        {
            return await _musicianOnConcertStorage.GetListAsync(null, null, token);
        }

        // Удаление музыканта с концерта
        public async Task DeleteMusicianOnConcertAsync(Guid concertId, Guid musicianId, CancellationToken token)
        {
            await _musicianOnConcertStorage.DeleteAsync($"ConcertId = {concertId} AND MusicianId = {musicianId}", null, token);
        }
    }
}