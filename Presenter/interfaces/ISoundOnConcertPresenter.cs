using Storage;

namespace Presenter;

public interface ISoundOnConcertPresenter
{
    public Task AddSoundOnConcertAsync(Guid concertId, Guid soundId, CancellationToken token);
    public Task<IReadOnlyCollection<SoundOnConcert>> GetSoundOnConcertAsync(CancellationToken token);
}