namespace Storage;

public interface ISoundOnConcertStorage
{
    public Task AddSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token);
    public Task DeleteSoundOnConcertAsync(SoundOnConcert soundOnConcert, CancellationToken token);
    public Task<IReadOnlyCollection<SoundOnConcert>> GetAllSoundOnConcertAsync(CancellationToken token);
}