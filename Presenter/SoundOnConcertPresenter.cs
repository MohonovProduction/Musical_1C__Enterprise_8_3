using Storage;

namespace Presenter;

public class SoundOnConcertPresenter : ISoundOnConcertPresenter
{
    private readonly SoundOnConcertStorage _instrumentStorage;

    public SoundOnConcertPresenter(SoundOnConcertStorage instrumentStorage)
    {
        _instrumentStorage = instrumentStorage;
    }

    public SoundOnConcertPresenter()
    {
        _instrumentStorage = new SoundOnConcertStorage("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c", "sound_on_concert");
    }
    
    public async Task AddSoundOnConcertAsync(Guid concertId, Guid soundId, CancellationToken token)
    {
        var instrument = new SoundOnConcert(concertId, soundId);
        await _instrumentStorage.AddSoundOnConcertAsync(instrument, token);
    }

    public async Task<IReadOnlyCollection<SoundOnConcert>> GetSoundOnConcertAsync(CancellationToken token)
    {
        return await _instrumentStorage.GetAllSoundOnConcertAsync(token);
    }
}