using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class MusicStorage : IMusicStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public MusicStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Добавление музыки (записи)
        public async Task AddMusicAsync(Sound sound, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _dbContext.Sounds.AddAsync(sound, token);
            await _dbContext.SaveChangesAsync(token);
        }

        // Удаление музыки (записи)
        public async Task DeleteMusicAsync(Sound sound, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var entity = await _dbContext.Sounds
                .FirstOrDefaultAsync(s => s.Id == sound.Id, token);

            if (entity != null)
            {
                _dbContext.Sounds.Remove(entity);
                await _dbContext.SaveChangesAsync(token);
            }
        }

        // Получение всех записей о музыке
        public async Task<IReadOnlyCollection<Sound>> GetAllMusicAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _dbContext.Sounds.ToListAsync(token);
        }
    }
}