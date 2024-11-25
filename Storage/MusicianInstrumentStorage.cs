using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class MusicianInstrumentStorage : IMusicianInstrumentStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public MusicianInstrumentStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление связи музыканта с инструментом
        public async Task AddMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _dbContext.MusicianInstruments.AddAsync(musicianInstrument, token);
            await _dbContext.SaveChangesAsync(token);
        }

        // Удаление связи музыканта с инструментом
        public async Task DeleteMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var entity = await _dbContext.MusicianInstruments
                .FirstOrDefaultAsync(
                    m => m.MusicianId == musicianInstrument.MusicianId &&
                         m.InstrumentId == musicianInstrument.InstrumentId, token);

            if (entity != null)
            {
                _dbContext.MusicianInstruments.Remove(entity);
                await _dbContext.SaveChangesAsync(token);
            }
        }

        // Получение всех связей музыкантов с инструментами
        public async Task<IReadOnlyCollection<MusicianInstrument>> GetAllMusicianInstrumentAsync(
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.MusicianInstruments.ToListAsync(token);
        }
    }
}