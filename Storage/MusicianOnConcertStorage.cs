using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class MusicianOnConcertStorage : IMusicianOnConcertStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public MusicianOnConcertStorage(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление музыканта на концерт
        public async Task AddMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                await dbContext.MusicianOnConcerts.AddAsync(musicianOnConcert, token);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Удаление музыканта с концерта
        public async Task DeleteMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                var entity = await dbContext.MusicianOnConcerts
                    .FirstOrDefaultAsync(
                        m => m.MusicianId == musicianOnConcert.MusicianId && m.ConcertId == musicianOnConcert.ConcertId,
                        token);

                if (entity != null)
                {
                    dbContext.MusicianOnConcerts.Remove(entity);
                    await dbContext.SaveChangesAsync(token);
                }
            }
        }

        // Получение всех музыкантов на концертах
        public async Task<IReadOnlyCollection<MusicianOnConcert>> GetAllMusicianOnConcertAsync(CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.MusicianOnConcerts.ToListAsync(token);
            }
        }

    }
}