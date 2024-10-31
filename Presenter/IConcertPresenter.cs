using Storage;

namespace Presenter;

public interface IConcertPresenter
{
   Task AddConcertAsync(string name, CancellationToken token);
    Task DeleteConcertAsync(Concert concert , CancellationToken token);
    Task<IReadOnlyCollection<Concert>> GetConcertsAsync(CancellationToken token);
}