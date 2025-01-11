using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class StorageDataBase<T> : IStorageDataBase<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public StorageDataBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        // Получение записи по условию
        public async Task<T> GetSingleAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken)
        {
            return await predicate(_dbSet).FirstOrDefaultAsync(cancellationToken);
        }

        // Получение списка записей по условию
        public async Task<IReadOnlyCollection<T>> GetListAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken)
        {
            return await predicate(_dbSet).ToListAsync(cancellationToken);
        }

        // Добавление записи в таблицу
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // Удаление записей по условию
        public async Task DeleteAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken)
        {
            var entitiesToDelete = predicate(_dbSet);
            _dbSet.RemoveRange(entitiesToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // Обновление записи по условию
        public async Task UpdateAsync(Func<IQueryable<T>, IQueryable<T>> predicate, T updatedEntity, CancellationToken cancellationToken)
        {
            var entityToUpdate = await predicate(_dbSet).FirstOrDefaultAsync(cancellationToken);
            if (entityToUpdate != null)
            {
                _dbContext.Entry(entityToUpdate).CurrentValues.SetValues(updatedEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}