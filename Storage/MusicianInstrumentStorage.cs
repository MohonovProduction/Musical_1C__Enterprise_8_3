namespace Storage;

public class MusicianInstrumentStorage
{
    private readonly StorageDataBase<MusicianInstrument> _storageDataBase;

    public MusicianInstrumentStorage(string connectionString, string tableName)
    {
        _storageDataBase = new StorageDataBase<MusicianInstrument>(connectionString, tableName);
    }

    public MusicianInstrumentStorage(StorageDataBase<MusicianInstrument> storageDataBase)
    {
        _storageDataBase = storageDataBase;
    }

    public async Task AddMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await _storageDataBase.AddAsync(musicianInstrument, token);
    }

    public async Task DeleteMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        // Удаление на основе ID или других уникальных полей инструмента
        await _storageDataBase.DeleteAsync(
            "id = @Id", 
            new 
            {
                MusicianId = musicianInstrument.MusicianId, 
                InstrumentId = musicianInstrument.InstrumentId
            }, 
            token);
    }
    
    public async Task<IReadOnlyCollection<MusicianInstrument>> GetAllMusicianInstrumentAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return await _storageDataBase.GetListAsync("", null, token); // Получаем все записи
    }
}