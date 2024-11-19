using Storage;

namespace Presenter;

public class MusicianOnConcertPresenter
{
    private readonly IMusicianOnConcertStorage _instrumentStorage;

    public MusicianOnConcertPresenter(IMusicianOnConcertStorage instrumentStorage)
    {
        _instrumentStorage = instrumentStorage;
    }

    public MusicianOnConcertPresenter()
    {
        _instrumentStorage = new MusicianOnConcertStorage("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c", "musician_on_concert");
    }
    
    public async Task AddMusicianOnConcertAsync(Guid concertId, Guid musicianId, CancellationToken token)
    {
        var instrument = new MusicianOnConcert(concertId, musicianId);
        await _instrumentStorage.AddMusicianOnConcertAsync(instrument, token);
    }

    public async Task<IReadOnlyCollection<MusicianOnConcert>> GetMusicianOnConcertAsync(CancellationToken token)
    {
        return await _instrumentStorage.GetAllMusicianOnConcertAsync(token);
    }
}