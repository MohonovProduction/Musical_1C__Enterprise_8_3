using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class SoundStorage : ISoundStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public SoundStorage(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление музыки (записи)
        public async Task AddMusicAsync(Sound sound, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                await dbContext.Sounds.AddAsync(sound, token);
                await dbContext.SaveChangesAsync(token);
            }
        }

        // Удаление музыки (записи)
        public async Task DeleteMusicAsync(Sound sound, CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                var entity = await dbContext.Sounds
                    .FirstOrDefaultAsync(s => s.Id == sound.Id, token);

                if (entity != null)
                {
                    dbContext.Sounds.Remove(entity);
                    await dbContext.SaveChangesAsync(token);
                }
            }
        }

        // Получение всех записей о музыке
        public async Task<IReadOnlyCollection<Sound>> GetAllMusicAsync(CancellationToken token)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                token.ThrowIfCancellationRequested();
                return await dbContext.Sounds.ToListAsync(token);
            }
        }

    }
}