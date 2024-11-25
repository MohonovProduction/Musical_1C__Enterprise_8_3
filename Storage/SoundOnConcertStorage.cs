using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class SoundOnConcertStorage : ISoundOnConcertStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public SoundOnConcertStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление связи между концертом и звуком
        public async Task AddSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _dbContext.SoundOnConcerts.AddAsync(soundOnConcert, token);
            await _dbContext.SaveChangesAsync(token);
        }

        // Удаление связи между концертом и звуком
        public async Task DeleteSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var entity = await _dbContext.SoundOnConcerts
                .FirstOrDefaultAsync(s => s.ConcertId == soundOnConcert.ConcertId && s.SoundId == soundOnConcert.SoundId, token);

            if (entity != null)
            {
                _dbContext.SoundOnConcerts.Remove(entity);
                await _dbContext.SaveChangesAsync(token);
            }
        }

        // Получение всех записей о связях между концертами и звуками
        public async Task<IReadOnlyCollection<SoundOnConcert>> GetAllSoundOnConcertAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.SoundOnConcerts.ToListAsync(token);
        }
    }
}