using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class InstrumentStorage : IInstrumentStorage
    {
        public IStorageFile<Instrument> _storageFile;

        public InstrumentStorage(string filePath, string tableName)
        {
            _storageFile = new StorageFile<Instrument>(filePath, tableName);
        }

        public InstrumentStorage(IStorageFile<Instrument> filePath)
        {
            _storageFile = new StorageFile<Instrument>("../../data/Instruments.json", "Instruments.json");
        }

        public async Task AddInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            await _storageFile.AddAsync(instrument, token);
        }

        public async Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            // Удаление на основе ID или других уникальных полей инструмента
            await _storageFile.DeleteAsync(i => i.Id == instrument.Id, token);
        }

        public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token)
        {
            var instruments = await _storageFile.GetAllAsync(token);
            return instruments.AsReadOnly();
        }
    }
}