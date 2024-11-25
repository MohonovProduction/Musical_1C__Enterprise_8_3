using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class MusicianOnConcertStorage : IMusicianOnConcertStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public MusicianOnConcertStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление музыканта на концерт
        public async Task AddMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _dbContext.MusicianOnConcerts.AddAsync(musicianOnConcert, token);
            await _dbContext.SaveChangesAsync(token);
        }

        // Удаление музыканта с концерта
        public async Task DeleteMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var entity = await _dbContext.MusicianOnConcerts
                .FirstOrDefaultAsync(m => m.MusicianId == musicianOnConcert.MusicianId && m.ConcertId == musicianOnConcert.ConcertId, token);
            
            if (entity != null)
            {
                _dbContext.MusicianOnConcerts.Remove(entity);
                await _dbContext.SaveChangesAsync(token);
            }
        }

        // Получение всех музыкантов на концертах
        public async Task<IReadOnlyCollection<MusicianOnConcert>> GetAllMusicianOnConcertAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.MusicianOnConcerts.ToListAsync(token);
        }
    }
}