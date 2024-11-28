namespace Storage;

public interface IStorageDataBase<T>
{
    public Task<T> GetSingleAsync(string whereClause, object parameters, CancellationToken cancellationToken);

    public Task<IReadOnlyCollection<T>> GetListAsync(string whereClause, object parameters,
        CancellationToken cancellationToken);

    public Task AddAsync(T entity, CancellationToken cancellationToken);
    
    public Task DeleteAsync(string whereClause, object parameters, CancellationToken cancellationToken);

    public Task UpdateAsync(string whereClause, object parameters, T updatedEntity,
        CancellationToken cancellationToken);
}