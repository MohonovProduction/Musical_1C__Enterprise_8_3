namespace Storage;

public interface IStorageDataBase<T> where T : class
{
    // Получение записи по условию
    Task<T> GetSingleAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken);

    // Получение списка записей по условию
    Task<IReadOnlyCollection<T>> GetListAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken);

    // Добавление записи в таблицу
    Task AddAsync(T entity, CancellationToken cancellationToken);

    // Удаление записей по условию
    Task DeleteAsync(Func<IQueryable<T>, IQueryable<T>> predicate, CancellationToken cancellationToken);

    // Обновление записи по условию
    Task UpdateAsync(Func<IQueryable<T>, IQueryable<T>> predicate, T updatedEntity, CancellationToken cancellationToken);
}