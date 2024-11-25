namespace Storage;

public interface IMusicStorage
{
    Task AddMusicAsync(Sound sound, CancellationToken token);
    Task DeleteMusicAsync(Sound sound, CancellationToken token);
    Task<IReadOnlyCollection<Sound>> GetAllMusicAsync(CancellationToken token);
}