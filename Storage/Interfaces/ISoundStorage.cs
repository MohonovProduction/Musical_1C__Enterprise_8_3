namespace Storage;

public interface ISoundStorage
{
    Task AddMusicAsync(Sound sound, CancellationToken token);
    Task DeleteMusicAsync(Sound sound, CancellationToken token);
    Task<IReadOnlyCollection<Sound>> GetAllMusicAsync(CancellationToken token);
}