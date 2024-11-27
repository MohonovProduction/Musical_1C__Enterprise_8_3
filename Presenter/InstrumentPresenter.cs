using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class InstrumentPresenter : IInstrumentPresenter
    {
        private readonly IStorageDataBase<Instrument> _instrumentStorage;

        // Конструктор, принимающий IStorageDataBase<Instrument>
        public InstrumentPresenter(IStorageDataBase<Instrument> instrumentStorage)
        {
            _instrumentStorage = instrumentStorage;
        }

        // Конструктор по умолчанию, использующий InstrumentStorage
        public InstrumentPresenter(ApplicationDbContext dbContext)
        {
            _instrumentStorage = new StorageDataBase<Instrument>(dbContext);
        }

        public InstrumentPresenter()
        {
        }

        // Добавление инструмента
        public async Task AddInstrumentAsync(Guid id, string name, CancellationToken token)
        {
            var instrument = new Instrument(id, name);
            await _instrumentStorage.AddAsync(instrument, token);
        }

        // Удаление инструмента
        public async Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            // Если необходимо, реализуйте логику удаления по ID или другим условиям
            await _instrumentStorage.DeleteAsync($"Id = {instrument.Id}", null, token);
        }

        // Получение всех инструментов
        public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token)
        {
            return await _instrumentStorage.GetListAsync(null, null, token);
        }

        // Получение инструмента по ID
        public async Task<Instrument> GetInstrumentByIdAsync(Guid id, CancellationToken token)
        {
            return await _instrumentStorage.GetSingleAsync($"Id = {id}", null, token);
        }
    }
}