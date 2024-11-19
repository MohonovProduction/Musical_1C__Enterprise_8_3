namespace Storage;

public interface IMusicStorage
{
    Task AddMusicAsync(Music music, CancellationToken token);
    Task DeleteMusicAsync(Music music, CancellationToken token);
    Task<IReadOnlyCollection<Music>> GetAllMusicAsync(CancellationToken token);
}