using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class SoundOnConcertStorage : ISoundOnConcertStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public SoundOnConcertStorage(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление связи между концертом и звуком
        public async Task AddSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                await dbContext.SoundOnConcerts.AddAsync(soundOnConcert, token);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Удаление связи между концертом и звуком
        public async Task DeleteSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                var entity = await dbContext.SoundOnConcerts
                    .FirstOrDefaultAsync(
                        s => s.ConcertId == soundOnConcert.ConcertId && s.SoundId == soundOnConcert.SoundId, token);

                if (entity != null)
                {
                    dbContext.SoundOnConcerts.Remove(entity);
                    await dbContext.SaveChangesAsync(token);
                }
            }
        }

        // Получение всех записей о связях между концертами и звуками
        public async Task<IReadOnlyCollection<SoundOnConcert>> GetAllSoundOnConcertAsync(CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.SoundOnConcerts.ToListAsync(token);
            }
        }

    }
}