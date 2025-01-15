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
        private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

        public StorageDataBase(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        // Получение записи по условию
        public async Task<T> GetSingleAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var dbSet = dbContext.Set<T>();
                return await predicate(dbSet).FirstOrDefaultAsync(cancellationToken);
            }
        }

        // Получение списка записей по условию
        public async Task<IReadOnlyCollection<T>> GetListAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var dbSet = dbContext.Set<T>();
                return await predicate(dbSet).ToListAsync(cancellationToken);
            }
        }

        // Добавление записи в таблицу
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var dbSet = dbContext.Set<T>();
                await dbSet.AddAsync(entity, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        // Удаление записей по условию
        public async Task DeleteAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var dbSet = dbContext.Set<T>();
                var entitiesToDelete = predicate(dbSet);
                dbSet.RemoveRange(entitiesToDelete);
                await dbContext.SaveChangesAsync(cancellationToken);   
            }
        }

        // Обновление записи по условию
        public async Task UpdateAsync(Func<IQueryable<T>, IQueryable<T>> predicate, T updatedEntity, CancellationToken cancellationToken)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var dbSet = dbContext.Set<T>();
                var entityToUpdate = await predicate(dbSet).FirstOrDefaultAsync(cancellationToken);
                if (entityToUpdate != null)
                {
                    dbContext.Entry(entityToUpdate).CurrentValues.SetValues(updatedEntity);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }   
            }
        }
    }
}