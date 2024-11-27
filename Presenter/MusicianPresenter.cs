using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class MusicianPresenter : IMusicianPresenter
    {
        private readonly IStorageDataBase<Musician> _musicianStorage;
        private readonly IStorageDataBase<Instrument> _instrumentStorage;
        private readonly IStorageDataBase<MusicianInstrument> _musicianInstrumentStorage;

        public MusicianPresenter(IStorageDataBase<Musician> musicianStorage, 
                                 IStorageDataBase<Instrument> instrumentStorage, 
                                 IStorageDataBase<MusicianInstrument> musicianInstrumentStorage)
        {
            _musicianStorage = musicianStorage;
            _instrumentStorage = instrumentStorage;
            _musicianInstrumentStorage = musicianInstrumentStorage;
        }

        public MusicianPresenter(ApplicationDbContext dbContext)
        {
            _musicianStorage = new StorageDataBase<Musician>(dbContext);
            _instrumentStorage = new StorageDataBase<Instrument>(dbContext);
            _musicianInstrumentStorage = new StorageDataBase<MusicianInstrument>(dbContext);
        }

        public MusicianPresenter() {}

        public async Task<Musician> AddMusicianAsync(string name, string lastName, string surname, 
                                                      List<Instrument> instruments, CancellationToken token)
        {
            var id = Guid.NewGuid();
            var musician = new Musician(id, name, lastName, surname);

            // Добавление музыканта в базу данных
            await _musicianStorage.AddAsync(musician, token);

            // Добавление инструментов
            foreach (var instrument in instruments)
            {
                // Добавление инструмента в базу данных
                await _instrumentStorage.AddAsync(instrument, token);

                // Связывание музыканта с инструментом
                var musicianInstrument = new MusicianInstrument(musician.Id, instrument.Id);
                await _musicianInstrumentStorage.AddAsync(musicianInstrument, token);
            }

            return musician; // Возвращаем добавленного музыканта
        }

        public async Task DeleteMusicianAsync(Musician musician, CancellationToken token)
        {
            await _musicianStorage.DeleteAsync($"Id = {musician.Id}", null, token);

            // Удаление связей музыканта с инструментами
            await _musicianInstrumentStorage.DeleteAsync($"MusicianId = {musician.Id}", null, token);
        }

        public async Task<IReadOnlyCollection<Musician>> GetMusiciansAsync(CancellationToken token)
        {
            return await _musicianStorage.GetListAsync(null, null, token);
        }
    }
}
