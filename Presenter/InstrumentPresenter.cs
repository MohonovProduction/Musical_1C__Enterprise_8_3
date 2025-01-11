using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class InstrumentPresenter : IInstrumentPresenter
    {
        private readonly IStorageDataBase<Instrument> _instrumentStorage;

        // Constructor expecting IStorageDataBase<Instrument>
        public InstrumentPresenter(IStorageDataBase<Instrument> instrumentStorage)
        {
            _instrumentStorage = instrumentStorage;
        }

        // Constructor expecting ApplicationDbContext
        public InstrumentPresenter(ApplicationDbContext dbContext)
        {
            _instrumentStorage = new StorageDataBase<Instrument>(dbContext);
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
            // Удаление инструмента по ID с использованием LINQ
            await _instrumentStorage.DeleteAsync(query => query.Where(i => i.Id == instrument.Id), token);
        }

        // Получение всех инструментов
        public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token)
        {
            return await _instrumentStorage.GetListAsync(query => query, token);
        }

        // Получение инструмента по ID
        public async Task<Instrument> GetInstrumentByIdAsync(Guid id, CancellationToken token)
        {
            return await _instrumentStorage.GetSingleAsync(query => query.Where(i => i.Id == id), token);
        }
    }
}