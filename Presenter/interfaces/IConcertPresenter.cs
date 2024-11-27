using Storage;

namespace Presenter;

public interface IConcertPresenter
{
   Task<bool> AddConcertAsync(string name, CancellationToken token);
    Task DeleteConcertAsync(Concert concert , CancellationToken token);
    Task<IReadOnlyCollection<Concert>> GetConcertsAsync(CancellationToken token);
}