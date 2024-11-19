namespace Storage;

public interface IConcertStorage
{
    Task AddConcertAsync(Concert concert, CancellationToken token);
    Task DeleteConcertAsync(Concert concert, CancellationToken token);
    Task<IReadOnlyCollection<Concert>> GetAllConcertsAsync(CancellationToken token);
}