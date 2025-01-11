using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class MusicianInstrumentPresenter : IMusicianInstrumentPresenter
    {
        private readonly IStorageDataBase<MusicianInstrument> _musicianInstrumentStorage;
        private readonly IStorageDataBase<Musician> _musicianStorage;
        private readonly IStorageDataBase<Instrument> _instrumentStorage;

        // Конструктор, который принимает IStorageDataBase для MusicianInstrument, Musician и Instrument
        public MusicianInstrumentPresenter(IStorageDataBase<MusicianInstrument> musicianInstrumentStorage,
                                           IStorageDataBase<Musician> musicianStorage,
                                           IStorageDataBase<Instrument> instrumentStorage)
        {
            _musicianInstrumentStorage = musicianInstrumentStorage;
            _musicianStorage = musicianStorage;
            _instrumentStorage = instrumentStorage;
        }

        // Конструктор, который принимает ApplicationDbContext
        public MusicianInstrumentPresenter(ApplicationDbContext dbContext)
        {
            _musicianInstrumentStorage = new StorageDataBase<MusicianInstrument>(dbContext);
            _musicianStorage = new StorageDataBase<Musician>(dbContext);
            _instrumentStorage = new StorageDataBase<Instrument>(dbContext);
        }

        // Добавление инструмента музыканту
        public async Task AddMusicianInstrumentAsync(Guid musicianId, Guid instrumentId, CancellationToken token)
        {
            var musician = await _musicianStorage.GetSingleAsync(query => query.Where(m => m.Id == musicianId), token);
            var instrument = await _instrumentStorage.GetSingleAsync(query => query.Where(i => i.Id == instrumentId), token);

            if (musician == null || instrument == null)
            {
                throw new ArgumentException("Musician or instrument not found.");
            }

            var musicianInstrument = new MusicianInstrument(musicianId, instrumentId, musician, instrument);
            await _musicianInstrumentStorage.AddAsync(musicianInstrument, token);
        }

        // Удаление инструмента у музыканта
        public async Task DeleteMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token)
        {
            await _musicianInstrumentStorage.DeleteAsync(query => query.Where(m => m.MusicianId == musicianInstrument.MusicianId && m.InstrumentId == musicianInstrument.InstrumentId), token);
        }

        // Получение всех инструментов музыканта
        public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsForMusicianAsync(Guid musicianId, CancellationToken token)
        {
            var musicianInstruments = await _musicianInstrumentStorage.GetListAsync(query => query.Where(m => m.MusicianId == musicianId), token);
            var instrumentIds = musicianInstruments.Select(m => m.InstrumentId).ToList();
            var instruments = await _instrumentStorage.GetListAsync(query => query.Where(i => instrumentIds.Contains(i.Id)), token);
            return instruments;
        }

        // Получение всех музыкантов, играющих на инструменте
        public async Task<IReadOnlyCollection<Musician>> GetMusiciansForInstrumentAsync(Guid instrumentId, CancellationToken token)
        {
            var musicianInstruments = await _musicianInstrumentStorage.GetListAsync(query => query.Where(m => m.InstrumentId == instrumentId), token);
            var musicianIds = musicianInstruments.Select(m => m.MusicianId).ToList();
            var musicians = await _musicianStorage.GetListAsync(query => query.Where(m => musicianIds.Contains(m.Id)), token);
            return musicians;
        }

        // Получение всех комбинаций музыкант-инструмент
        public async Task<IReadOnlyCollection<MusicianInstrument>> GetMusicianInstrumentsAsync(CancellationToken token)
        {
            return await _musicianInstrumentStorage.GetListAsync(query => query, token);
        }

        // Получение комбинации музыканта и инструмента по ID музыканта и инструмента
        public async Task<MusicianInstrument> GetMusicianInstrumentByIdsAsync(Guid musicianId, Guid instrumentId, CancellationToken token)
        {
            return await _musicianInstrumentStorage.GetSingleAsync(query => query.Where(m => m.MusicianId == musicianId && m.InstrumentId == instrumentId), token);
        }
    }
}
