namespace Storage;

public interface IMusicianInstrumentStorage
{
    public Task AddMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token);
    public Task DeleteMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token);
    public Task<IReadOnlyCollection<MusicianInstrument>> GetAllMusicianInstrumentAsync(CancellationToken token);
}