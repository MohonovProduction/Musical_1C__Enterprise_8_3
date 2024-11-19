namespace Storage;

public class MusicianOnConcertStorage : IMusicianOnConcertStorage
{
    private readonly StorageDataBase<MusicianOnConcert> _storageDataBase;

    public MusicianOnConcertStorage(string connectionString, string tableName)
    {
        _storageDataBase = new StorageDataBase<MusicianOnConcert>(connectionString, tableName);
    }

    public MusicianOnConcertStorage(StorageDataBase<MusicianOnConcert> storageDataBase)
    {
        _storageDataBase = storageDataBase;
    }

    public async Task AddMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await _storageDataBase.AddAsync(musicianOnConcert, token);
    }

    public async Task DeleteMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        // Удаление на основе ID или других уникальных полей инструмента
        await _storageDataBase.DeleteAsync(
            "id = @Id", 
            new 
            {
                MusicianId = musicianOnConcert.MusicianId, 
                ConcertId = musicianOnConcert.ConcertId
            }, 
            token);
    }
    
    public async Task<IReadOnlyCollection<MusicianOnConcert>> GetAllMusicianOnConcertAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return await _storageDataBase.GetListAsync("", null, token); // Получаем все записи
    }
}