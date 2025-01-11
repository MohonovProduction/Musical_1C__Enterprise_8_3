using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Presenter
{
    public class MusicianPresenter : IMusicianPresenter
    {
        private readonly IStorageDataBase<Musician> _musicianStorage;
        private readonly IStorageDataBase<Instrument> _instrumentStorage;
        private readonly IStorageDataBase<MusicianInstrument> _musicianInstrumentStorage;
        private readonly IStorageDataBase<MusicianOnConcert> _musicianOnConcertStorage;

        // Конструктор, который принимает IStorageDataBase<Musician>
        public MusicianPresenter(IStorageDataBase<Musician> musicianStorage, 
                                 IStorageDataBase<Instrument> instrumentStorage, 
                                 IStorageDataBase<MusicianInstrument> musicianInstrumentStorage,
                                  IStorageDataBase<MusicianOnConcert> musicianOnConcertStorage)
        {
            _musicianStorage = musicianStorage;
            _instrumentStorage = instrumentStorage;
            _musicianInstrumentStorage = musicianInstrumentStorage;
            _musicianOnConcertStorage = musicianOnConcertStorage;
        }

        // Конструктор, который принимает ApplicationDbContext
        public MusicianPresenter(ApplicationDbContext dbContext)
        {
            _musicianStorage = new StorageDataBase<Musician>(dbContext);
            _instrumentStorage = new StorageDataBase<Instrument>(dbContext);
            _musicianInstrumentStorage = new StorageDataBase<MusicianInstrument>(dbContext);
            _musicianOnConcertStorage = new StorageDataBase<MusicianOnConcert>(dbContext);
        }

        // Добавление музыканта с инструментами
        public async Task<Musician> AddMusicianAsync(string name, string lastName, string surname, 
                                                      List<Instrument> instruments, List<Concert> concerts, CancellationToken token)
        {
            var id = Guid.NewGuid();
            var musician = new Musician(id, name, lastName, surname);

            // Добавление музыканта в базу данных
            await _musicianStorage.AddAsync(musician, token);

            // Добавление инструментов и создание связей
            foreach (var instrument in instruments)
            {
                // Добавление инструмента в базу данных
                await _instrumentStorage.AddAsync(instrument, token);

                // Связывание музыканта с инструментом
                var musicianInstrument = new MusicianInstrument(musician.Id, instrument.Id);
                await _musicianInstrumentStorage.AddAsync(musicianInstrument, token);
            }
            
            // Создание связей с концертом
            foreach (var concert in concerts)
            {
                var musicianOnConcert = new MusicianOnConcert(musician.Id, concert.Id);
                await _musicianOnConcertStorage.AddAsync(musicianOnConcert, token);
            }

            return musician; // Возвращаем добавленного музыканта
        }

        // Удаление музыканта и его связей с инструментами
        public async Task DeleteMusicianAsync(Musician musician, CancellationToken token)
        {
            // Удаление музыканта
            await _musicianStorage.DeleteAsync(query => query.Where(m => m.Id == musician.Id), token);

            // Удаление связей музыканта с инструментами
            await _musicianInstrumentStorage.DeleteAsync(query => query.Where(m => m.MusicianId == musician.Id), token);
        }

        // Получение списка музыкантов с их инструментами и концертами
        public async Task<IReadOnlyCollection<Musician>> GetMusiciansAsync(CancellationToken token)
        {
            return await _musicianStorage.GetListAsync(query => 
                    query.Include(m => m.MusicianInstruments)
                        .ThenInclude(mi => mi.Instrument) // Включаем инструменты музыканта
                        .Include(m => m.MusicianOnConcerts)
                        .ThenInclude(moc => moc.Concert), // Включаем концерты музыканта
                token);
        }

// Получение музыканта по ID с его инструментами и концертами
        public async Task<Musician> GetMusicianByIdAsync(Guid id, CancellationToken token)
        {
            return await _musicianStorage.GetSingleAsync(query => 
                    query.Where(m => m.Id == id)
                        .Include(m => m.MusicianInstruments)
                        .ThenInclude(mi => mi.Instrument) // Включаем инструменты музыканта
                        .Include(m => m.MusicianOnConcerts)
                        .ThenInclude(moc => moc.Concert), // Включаем концерты музыканта
                token);
        }

    }
}
