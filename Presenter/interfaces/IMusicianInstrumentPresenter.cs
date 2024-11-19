using Storage;

namespace Presenter;

public interface IMusicianInstrumentPresenter
{
    public Task AddMusicianInstrumentAsync(Guid musicianId, Guid instrumentId, CancellationToken token);
    public Task<IReadOnlyCollection<MusicianInstrument>> GetMusicianInstrumentAsync(CancellationToken token);
}