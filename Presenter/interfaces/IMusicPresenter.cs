using Storage;

namespace Presenter;

public interface IMusicPresenter
{
    Task<Sound> AddMusicAsync(string name, string author, CancellationToken token);
    Task DeleteMusicAsync(Sound sound , CancellationToken token);
    Task<IReadOnlyCollection<Sound>> GetMusicAsync(CancellationToken token);
}