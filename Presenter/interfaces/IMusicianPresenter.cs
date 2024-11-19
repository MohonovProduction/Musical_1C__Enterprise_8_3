using Storage;

namespace Presenter;

public interface IMusicianPresenter
{
    Task<Musician> AddMusicianAsync(string name,string lastName, string surname,List<Storage.Instrument> instruments, CancellationToken token);
    Task DeleteMusicianAsync(Musician musician , CancellationToken token);
    Task<IReadOnlyCollection<Musician>> GetMusiciansAsync(CancellationToken token);
}