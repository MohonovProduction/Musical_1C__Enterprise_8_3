using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class MusicianStorage : IMusicianStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public MusicianStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление музыканта
        public async Task AddMusicianAsync(Musician musician, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _dbContext.Musicians.AddAsync(musician, token);
            await _dbContext.SaveChangesAsync(token);
        }

        // Удаление музыканта
        public async Task DeleteMusicianAsync(Musician musician, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var entity = await _dbContext.Musicians
                .FirstOrDefaultAsync(m => m.Id == musician.Id, token);

            if (entity != null)
            {
                _dbContext.Musicians.Remove(entity);
                await _dbContext.SaveChangesAsync(token);
            }
        }

        // Получение всех музыкантов
        public async Task<IReadOnlyCollection<Musician>> GetAllMusiciansAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.Musicians.ToListAsync(token);
        }
    }
}