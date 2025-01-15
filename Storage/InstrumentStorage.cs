using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class InstrumentStorage : IInstrumentStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public InstrumentStorage(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление инструмента
        public async Task AddInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                await dbContext.Instruments.AddAsync(instrument, token);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Удаление инструмента
        public async Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                dbContext.Instruments.Remove(instrument);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Получение всех инструментов
        public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.Instruments.ToListAsync(token);
            }
        }

        // Получение инструмента по ID
        public async Task<Instrument> GetInstrumentByIdAsync(Guid id, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.Instruments.FirstOrDefaultAsync(i => i.Id == id, token);
            }
        }

    }
}