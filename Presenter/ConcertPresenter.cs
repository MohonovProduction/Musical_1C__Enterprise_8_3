using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Presenter
{
    public class ConcertPresenter : IConcertPresenter
    {
        private readonly IStorageDataBase<Concert> _concertStorage;
        private readonly IStorageDataBase<MusicianOnConcert> _musicianOnConcertStorage;
        private readonly IStorageDataBase<SoundOnConcert> _soundOnConcertStorage;

        // Конструктор, который принимает IStorageDataBase<Concert>
        public ConcertPresenter(IStorageDataBase<Concert> concertStorage, 
                                IStorageDataBase<MusicianOnConcert> musicianOnConcertStorage,
                                IStorageDataBase<SoundOnConcert> soundOnConcertStorage)
        {
            _concertStorage = concertStorage;
            _musicianOnConcertStorage = musicianOnConcertStorage;
            _soundOnConcertStorage = soundOnConcertStorage;
        }

        // Конструктор, который принимает ApplicationDbContext
        public ConcertPresenter(ApplicationDbContext dbContext)
        {
            _concertStorage = new StorageDataBase<Concert>(dbContext);
            _musicianOnConcertStorage = new StorageDataBase<MusicianOnConcert>(dbContext);
            _soundOnConcertStorage = new StorageDataBase<SoundOnConcert>(dbContext);
        }

        // Добавление концерта с музыкантами и произведениями
        public async Task<Concert> AddConcertAsync(string name, string type, string date, 
                                                   List<Musician> musicians, List<Sound> sounds, 
                                                   CancellationToken token)
        {
            var id = Guid.NewGuid();
            var concert = new Concert(id, name, type, date);

            // Добавление концерта в базу данных
            await _concertStorage.AddAsync(concert, token);

            // Добавление музыкантов на концерт
            foreach (var musician in musicians)
            {
                var musicianOnConcert = new MusicianOnConcert(concert.Id, musician.Id);
                await _musicianOnConcertStorage.AddAsync(musicianOnConcert, token);
            }

            // Добавление произведений на концерт
            foreach (var sound in sounds)
            {
                var soundOnConcert = new SoundOnConcert(concert.Id, sound.Id);
                await _soundOnConcertStorage.AddAsync(soundOnConcert, token);
            }

            return concert; // Возвращаем добавленный концерт
        }

        // Удаление концерта и его связей с музыкантами и произведениями
        public async Task DeleteConcertAsync(Concert concert, CancellationToken token)
        {
            // Удаление концерта
            await _concertStorage.DeleteAsync(query => query.Where(c => c.Id == concert.Id), token);

            // Удаление связей с музыкантами
            await _musicianOnConcertStorage.DeleteAsync(query => query.Where(moc => moc.ConcertId == concert.Id), token);

            // Удаление связей с произведениями
            await _soundOnConcertStorage.DeleteAsync(query => query.Where(soc => soc.ConcertId == concert.Id), token);
        }

        // Получение списка всех концертов с музыкантами и произведениями
        public async Task<IReadOnlyCollection<Concert>> GetConcertsAsync(CancellationToken token)
        {
            return await _concertStorage.GetListAsync(query => 
                    query.Include(c => c.MusicianOnConcerts)
                        .ThenInclude(moc => moc.Musician) // Включаем музыкантов концерта
                        .Include(c => c.SoundOnConcerts)
                        .ThenInclude(soc => soc.Sound), // Включаем произведения концерта
                token);
        }

        // Получение концерта по ID с музыкантами и произведениями
        public async Task<Concert> GetConcertByIdAsync(Guid id, CancellationToken token)
        {
            return await _concertStorage.GetSingleAsync(query => 
                    query.Where(c => c.Id == id)
                        .Include(c => c.MusicianOnConcerts)
                        .ThenInclude(moc => moc.Musician) // Включаем музыкантов концерта
                        .Include(c => c.SoundOnConcerts)
                        .ThenInclude(soc => soc.Sound), // Включаем произведения концерта
                token);
        }

        // Добавление музыканта на концерт
        public async Task AddMusicianToConcertAsync(Concert concert, Musician musician, CancellationToken token)
        {
            var musicianOnConcert = new MusicianOnConcert(musician.Id, concert.Id);
            await _musicianOnConcertStorage.AddAsync(musicianOnConcert, token);
        }

        // Удаление музыканта с концерта
        public async Task RemoveMusicianFromConcertAsync(Concert concert, Musician musician, CancellationToken token)
        {
            await _musicianOnConcertStorage.DeleteAsync(query => 
                query.Where(moc => moc.ConcertId == concert.Id && moc.MusicianId == musician.Id), token);
        }

        // Добавление произведения (звука) на концерт
        public async Task AddSoundToConcertAsync(Concert concert, Sound sound, CancellationToken token)
        {
            var soundOnConcert = new SoundOnConcert(concert.Id, sound.Id);
            await _soundOnConcertStorage.AddAsync(soundOnConcert, token);
        }

        // Удаление произведения (звука) с концерта
        public async Task RemoveSoundFromConcertAsync(Concert concert, Sound sound, CancellationToken token)
        {
            await _soundOnConcertStorage.DeleteAsync(query => 
                query.Where(soc => soc.ConcertId == concert.Id && soc.SoundId == sound.Id), token);
        }
    }
}
