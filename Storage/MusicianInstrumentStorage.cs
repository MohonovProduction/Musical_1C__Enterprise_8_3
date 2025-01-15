using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class MusicianInstrumentStorage : IMusicianInstrumentStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public MusicianInstrumentStorage(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление связи музыканта с инструментом
        public async Task AddMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                await dbContext.MusicianInstruments.AddAsync(musicianInstrument, token);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Удаление связи музыканта с инструментом
        public async Task DeleteMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                var entity = await dbContext.MusicianInstruments
                    .FirstOrDefaultAsync(
                        m => m.MusicianId == musicianInstrument.MusicianId &&
                             m.InstrumentId == musicianInstrument.InstrumentId, token);

                if (entity != null)
                {
                    dbContext.MusicianInstruments.Remove(entity);
                    await dbContext.SaveChangesAsync(token);
                }
            }
        }

        // Получение всех связей музыкантов с инструментами
        public async Task<IReadOnlyCollection<MusicianInstrument>> GetAllMusicianInstrumentAsync(CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.MusicianInstruments.ToListAsync(token);
            }
        }

    }
}