using Storage;

namespace Presenter;

public interface IMusicianOnConcertPresenter
{
    public Task AddMusicianOnConcertAsync(Guid concertId, Guid musicianId, CancellationToken token);
    public Task<IReadOnlyCollection<MusicianOnConcert>> GetMusicianOnConcertAsync(CancellationToken token);
}