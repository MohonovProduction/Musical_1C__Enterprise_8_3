namespace Storage;

public class SoundOnConcertStorage
{
    private readonly StorageDataBase<SoundOnConcert> _storageDataBase;

    public SoundOnConcertStorage(string connectionString, string tableName)
    {
        _storageDataBase = new StorageDataBase<SoundOnConcert>(connectionString, tableName);
    }

    public SoundOnConcertStorage(StorageDataBase<SoundOnConcert> storageDataBase)
    {
        _storageDataBase = storageDataBase;
    }

    public async Task AddSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await _storageDataBase.AddAsync(soundOnConcert, token);
    }

    public async Task DeleteSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        // Удаление на основе ID или других уникальных полей инструмента
        await _storageDataBase.DeleteAsync(
            "id = @Id", 
            new 
            {
                ConcertId = soundOnConcert.ConcertId, 
                SoundId = soundOnConcert.SoundId
            }, 
            token);
    }
    
    public async Task<IReadOnlyCollection<SoundOnConcert>> GetAllSoundOnConcertAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return await _storageDataBase.GetListAsync("", null, token); // Получаем все записи
    }
}