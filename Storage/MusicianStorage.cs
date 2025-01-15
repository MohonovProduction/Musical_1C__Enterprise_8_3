using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class MusicianStorage : IMusicianStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public MusicianStorage(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление музыканта
        public async Task AddMusicianAsync(Musician musician, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                await dbContext.Musicians.AddAsync(musician, token);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Удаление музыканта
        public async Task DeleteMusicianAsync(Musician musician, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                var entity = await dbContext.Musicians
                    .FirstOrDefaultAsync(m => m.Id == musician.Id, token);

                if (entity != null)
                {
                    dbContext.Musicians.Remove(entity);
                    await dbContext.SaveChangesAsync(token);
                }
            }
        }

        // Получение всех музыкантов
        public async Task<IReadOnlyCollection<Musician>> GetAllMusiciansAsync(CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.Musicians.ToListAsync(token);
            }
        }

    }
}