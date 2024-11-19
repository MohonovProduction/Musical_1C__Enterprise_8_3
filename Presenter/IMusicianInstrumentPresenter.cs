namespace Presenter;

public interface IMusicianInstrumentPresenter
{
    public Task AddMusicianInstrumentAsync(Guid musicianId, Guid instrumentId, CancellationToken token);
}