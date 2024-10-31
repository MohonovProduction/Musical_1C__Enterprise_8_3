using Storage;

namespace Presenter;

public interface IMusicPresenter
{
    Task<Music> AddMusicAsync(string name, string author, CancellationToken token);
    Task DeleteMusicAsync(Music music , CancellationToken token);
    Task<IReadOnlyCollection<Music>> GetMusicAsync(CancellationToken token);
}