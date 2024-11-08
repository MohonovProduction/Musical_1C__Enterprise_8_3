using Storage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter;

public class MusicianPresenter : IMusicianPresenter
{
    private readonly IMusicianStorage _musicianStorage;
    
    public MusicianPresenter(IMusicianStorage musicianStorage)
    {
        _musicianStorage = musicianStorage;
    }
    public MusicianPresenter()
    {
        _musicianStorage = new MusicianStorage("../../data/musicians.json", "musicians.json");
    }
    
    public async Task<Musician> AddMusicianAsync(string name, string lastName, string surname, List<Instrument> instruments, CancellationToken token)
    {
        var id = Guid.NewGuid();
        var musician = new Musician(id, name, lastName, surname, instruments);
        await _musicianStorage.AddMusicianAsync(musician, token);
        return musician; // Возвращаем добавленного музыканта
    }

    public async Task DeleteMusicianAsync(Musician musician, CancellationToken token)
    {
        await _musicianStorage.DeleteMusicianAsync(musician, token);
    }

    public async Task<IReadOnlyCollection<Musician>> GetMusiciansAsync(CancellationToken token)
    {
        return await _musicianStorage.GetAllMusiciansAsync(token);
    }
}