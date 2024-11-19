using Storage;

namespace Presenter;

public class MusicianInstrumentPresenter : IMusicianInstrumentPresenter
{
    private readonly MusicianInstrumentStorage _instrumentStorage;

    public MusicianInstrumentPresenter(MusicianInstrumentStorage instrumentStorage)
    {
        _instrumentStorage = instrumentStorage;
    }

    public MusicianInstrumentPresenter()
    {
        _instrumentStorage = new MusicianInstrumentStorage("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c", "musician_instrument");
    }
    
    public async Task AddMusicianInstrumentAsync(Guid musicianId, Guid instrumentId, CancellationToken token)
    {
        var instrument = new MusicianInstrument(musicianId, instrumentId);
        await _instrumentStorage.AddMusicianInstrumentAsync(instrument, token);
    }

    public async Task<IReadOnlyCollection<MusicianInstrument>> GetMusicianInstrumentAsync(CancellationToken token)
    {
        return await _instrumentStorage.GetAllMusicianInstrumentAsync(token);
    }
}