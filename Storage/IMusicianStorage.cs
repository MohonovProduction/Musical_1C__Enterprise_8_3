namespace Storage;

public interface IMusicianStorage
{
    Task AddMusicianAsync(Musician musician, CancellationToken token);
    Task DeleteMusicianAsync(Musician musician, CancellationToken token);
    Task<IReadOnlyCollection<Musician>> GetAllMusiciansAsync(CancellationToken token);
}