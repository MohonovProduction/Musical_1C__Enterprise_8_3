using Storage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter;

public class MusicianPresenter : IMusicianPresenter
{
    private readonly IMusicianStorage _musicianStorage;
    public InstrumentPresenter _instrumentPresenter = new InstrumentPresenter();
    public MusicianInstrumentPresenter _musicianInstrumentPresenter;
    
    public MusicianPresenter(IMusicianStorage Storage)
    {
        _musicianStorage = Storage;
    }
    public MusicianPresenter()
    {
        _musicianStorage = new MusicianStorage("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c", "musician");
    }
    
    public async Task<Musician> AddMusicianAsync(string name, string lastName, string surname, List<Instrument> instruments, CancellationToken token)
    {
        var id = Guid.NewGuid();
        var musician = new Musician(id, name, lastName, surname);
        await _musicianStorage.AddMusicianAsync(musician, token);
        
        foreach (var instrument in instruments)
        {
            await _instrumentPresenter.AddInstrumentAsync(instrument.Id, instrument.Name, token);
            await _musicianInstrumentPresenter.AddMusicianInstrumentAsync(musician.Id, instrument.Id, token);
        }
        
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