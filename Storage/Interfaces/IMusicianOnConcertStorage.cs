namespace Storage;

public interface IMusicianOnConcertStorage
{
    public Task AddMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token);
    public Task DeleteMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token);
    public Task<IReadOnlyCollection<MusicianOnConcert>> GetAllMusicianOnConcertAsync(CancellationToken token);
}