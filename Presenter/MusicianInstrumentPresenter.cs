using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class MusicianInstrumentPresenter : IMusicianInstrumentPresenter
    {
        private readonly IStorageDataBase<MusicianInstrument> _musicianInstrumentStorage;

        // Конструктор, принимающий IStorageDataBase<MusicianInstrument>
        public MusicianInstrumentPresenter(IStorageDataBase<MusicianInstrument> musicianInstrumentStorage)
        {
            _musicianInstrumentStorage = musicianInstrumentStorage;
        }

        // Конструктор по умолчанию, использующий MusicianInstrumentStorage
        public MusicianInstrumentPresenter(ApplicationDbContext dbContext)
        {
            _musicianInstrumentStorage = new StorageDataBase<MusicianInstrument>(dbContext);
        }

        public MusicianInstrumentPresenter()
        {
        }

        // Добавление связи музыканта с инструментом
        public async Task AddMusicianInstrumentAsync(Guid musicianId, Guid instrumentId, CancellationToken token)
        {
            var musicianInstrument = new MusicianInstrument(musicianId, instrumentId);
            await _musicianInstrumentStorage.AddAsync(musicianInstrument, token);
        }

        // Получение всех связей музыкантов с инструментами
        public async Task<IReadOnlyCollection<MusicianInstrument>> GetMusicianInstrumentAsync(CancellationToken token)
        {
            return await _musicianInstrumentStorage.GetListAsync(null, null, token);
        }

        // Удаление связи музыканта с инструментом
        public async Task DeleteMusicianInstrumentAsync(Guid musicianId, Guid instrumentId, CancellationToken token)
        {
            // Это можно изменить на более гибкий метод, если требуется
            await _musicianInstrumentStorage.DeleteAsync($"MusicianId = {musicianId} AND InstrumentId = {instrumentId}", null, token);
        }
    }
}