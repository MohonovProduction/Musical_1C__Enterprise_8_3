using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class InstrumentStorage : IInstrumentStorage
    {
        private readonly StorageDataBase<Instrument> _storageDataBase;

        public InstrumentStorage(string connectionString, string tableName)
        {
            _storageDataBase = new StorageDataBase<Instrument>(connectionString, tableName);
        }

        public InstrumentStorage(StorageDataBase<Instrument> storageDataBase)
        {
            _storageDataBase = storageDataBase;
        }

        public async Task AddInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _storageDataBase.AddAsync(instrument, token);
        }

        public async Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            // Удаление на основе ID или других уникальных полей инструмента
            await _storageDataBase.DeleteAsync("id = @Id", new { Id = instrument.Id }, token);
        }

        public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _storageDataBase.GetListAsync("", null, token); // Получаем все записи
        }
    }
}