using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Presenter
{
    public class MusicianOnConcertPresenter : IMusicianOnConcertPresenter
    {
        private readonly IStorageDataBase<MusicianOnConcert> _musicianOnConcertStorage;
        private readonly IStorageDataBase<Musician> _musicianStorage;
        private readonly IStorageDataBase<Concert> _concertStorage;

        // Конструктор, который принимает IStorageDataBase для MusicianOnConcert, Musician и Concert
        public MusicianOnConcertPresenter(IStorageDataBase<MusicianOnConcert> musicianOnConcertStorage,
                                          IStorageDataBase<Musician> musicianStorage,
                                          IStorageDataBase<Concert> concertStorage)
        {
            _musicianOnConcertStorage = musicianOnConcertStorage;
            _musicianStorage = musicianStorage;
            _concertStorage = concertStorage;
        }

        // Конструктор, который принимает ApplicationDbContext
        public MusicianOnConcertPresenter(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _musicianOnConcertStorage = new StorageDataBase<MusicianOnConcert>(dbContext);
            _musicianStorage = new StorageDataBase<Musician>(dbContext);
            _concertStorage = new StorageDataBase<Concert>(dbContext);   
        }

        // Добавление музыканта на концерт
        public async Task AddMusicianOnConcertAsync(Guid concertId, Guid musicianId, CancellationToken token)
        {
            var musician = await _musicianStorage.GetSingleAsync(query => query.Where(m => m.Id == musicianId), token);
            var concert = await _concertStorage.GetSingleAsync(query => query.Where(c => c.Id == concertId), token);

            if (musician == null || concert == null)
            {
                throw new ArgumentException("Musician or concert not found.");
            }

            var musicianOnConcert = new MusicianOnConcert(concertId, musicianId);
            await _musicianOnConcertStorage.AddAsync(musicianOnConcert, token);
        }

        // Удаление музыканта с концерта
        public async Task DeleteMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token)
        {
            await _musicianOnConcertStorage.DeleteAsync(query => query.Where(m => m.ConcertId == musicianOnConcert.ConcertId && m.MusicianId == musicianOnConcert.MusicianId), token);
        }

        // Получение всех музыкантов на концерте
        public async Task<IReadOnlyCollection<Musician>> GetMusiciansOnConcertAsync(Guid concertId, CancellationToken token)
        {
            var musicianOnConcerts = await _musicianOnConcertStorage.GetListAsync(query => query.Where(m => m.ConcertId == concertId), token);
            var musicianIds = musicianOnConcerts.Select(m => m.MusicianId).ToList();
            var musicians = await _musicianStorage.GetListAsync(query => query.Where(m => musicianIds.Contains(m.Id)), token);
            return musicians;
        }

        // Получение всех концертов музыканта
        public async Task<IReadOnlyCollection<Concert>> GetConcertsForMusicianAsync(Guid musicianId, CancellationToken token)
        {
            var musicianOnConcerts = await _musicianOnConcertStorage.GetListAsync(query => query.Where(m => m.MusicianId == musicianId), token);
            var concertIds = musicianOnConcerts.Select(m => m.ConcertId).ToList();
            var concerts = await _concertStorage.GetListAsync(query => query.Where(c => concertIds.Contains(c.Id)), token);
            return concerts;
        }

        // Получение всех музыкантов на концертах
        public async Task<IReadOnlyCollection<MusicianOnConcert>> GetMusiciansOnConcertsAsync(CancellationToken token)
        {
            return await _musicianOnConcertStorage.GetListAsync(query => query, token);
        }

        // Получение музыканта на концерте по ID концерта и музыканта
        public async Task<MusicianOnConcert> GetMusicianOnConcertByIdsAsync(Guid concertId, Guid musicianId, CancellationToken token)
        {
            return await _musicianOnConcertStorage.GetSingleAsync(query => query.Where(m => m.ConcertId == concertId && m.MusicianId == musicianId), token);
        }
    }
}
