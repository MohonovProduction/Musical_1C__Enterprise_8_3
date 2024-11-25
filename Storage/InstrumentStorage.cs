using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class InstrumentStorage : IInstrumentStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public InstrumentStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление инструмента
        public async Task AddInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _dbContext.Instruments.AddAsync(instrument, token);
            await _dbContext.SaveChangesAsync(token);
        }

        // Удаление инструмента
        public async Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            _dbContext.Instruments.Remove(instrument);
            await _dbContext.SaveChangesAsync(token);
        }

        // Получение всех инструментов
        public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.Instruments.ToListAsync(token);
        }

        // Получение инструмента по ID
        public async Task<Instrument> GetInstrumentByIdAsync(Guid id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.Instruments.FirstOrDefaultAsync(i => i.Id == id, token);
        }
    }
}