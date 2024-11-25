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
        public async Task<T> GetSingleAsync(string whereClause, object parameters, CancellationToken cancellationToken)
        {
            // Здесь вместо whereClause лучше использовать LINQ для фильтрации.
            throw new NotImplementedException("Use LINQ to build queries instead of SQL strings.");
        }

        // Получение списка записей по условию
        public async Task<IReadOnlyCollection<T>> GetListAsync(string whereClause, object parameters,
            CancellationToken cancellationToken)
        {
            // Если требуется универсальная фильтрация, реализуйте парсер whereClause, но рекомендуется использовать LINQ.
            if (string.IsNullOrEmpty(whereClause))
                return await _dbSet.ToListAsync(cancellationToken);

            throw new NotImplementedException("Use LINQ to build queries instead of SQL strings.");
        }

        // Добавление записи в таблицу
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // Удаление записей по условию
        public async Task DeleteAsync(string whereClause, object parameters, CancellationToken cancellationToken)
        {
            // Здесь также лучше использовать LINQ.
            throw new NotImplementedException("Use LINQ to specify which entities to delete.");
        }

        // Обновление записи по условию
        public async Task UpdateAsync(string whereClause, object parameters, T updatedEntity,
            CancellationToken cancellationToken)
        {
            // EF автоматически отслеживает изменения объектов, поэтому их можно обновить просто изменением свойств.
            throw new NotImplementedException("Use EF tracking to manage updates.");
        }
    }
}