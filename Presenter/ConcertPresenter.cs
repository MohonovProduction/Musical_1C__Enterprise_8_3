using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter;

public class ConcertPresenter : IConcertPresenter
{
    private readonly MusicianOnConcertPresenter _musicianOnConcertPresenter = new MusicianOnConcertPresenter();
    private readonly SoundOnConcertPresenter _soundOnConcertPresenter = new SoundOnConcertPresenter();
    private readonly ConcertStorage _concertStorage = new ConcertStorage("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c", "concert");
    private ConcertBuilder _concertBuilder = new ConcertBuilder();

    public async Task AddConcertAsync(string name,CancellationToken token)
    {
        var fullConcert = _concertBuilder.BuildConcert(name);
        
        if (fullConcert != null)
        {
            var concert = new Concert(Guid.NewGuid(), fullConcert.Name, fullConcert.Type, fullConcert.Date);
            await _concertStorage.AddConcertAsync(concert, token);

            foreach (var music in fullConcert.Music)
            {
                token = new CancellationToken();
                await _soundOnConcertPresenter.AddSoundOnConcertAsync(concert.Id, music.Id, token);
            }

            foreach (var musician in fullConcert.Musicians)
            {
                token = new CancellationToken();
                await _musicianOnConcertPresenter.AddMusicianOnConcertAsync(concert.Id, musician.Id, token);
            }
            
            _concertBuilder = new ConcertBuilder(); // сброс builder после сохранения
        }
        else
        {
            Console.WriteLine("Ошибка: не все данные концерта заполнены.");
        }
    }

    public async Task DeleteConcertAsync(Concert concert, CancellationToken token)
    {
        await _concertStorage.DeleteConcertAsync(concert, token);
    }

    public async Task<IReadOnlyCollection<Concert>> GetConcertsAsync(CancellationToken token)
    {
        return await _concertStorage.GetAllConcertsAsync(token);
    }

    public void SetConcertType(string type)
    {
        _concertBuilder.Type = type;
    }

    public void AddMusicToConcert(Sound sound)
    {
        _concertBuilder.Music.Add(sound);
    }

    public void AddMusicianToConcert(Musician musician)
    {
        _concertBuilder.Musicians.Add(musician);
    }

    public void SetConcertDate(string date)
    {
        _concertBuilder.Date = date;
    }

    public async Task<ConcertBuilder> GetConcertBuilderAsync()
    {
        return _concertBuilder;
    }
   
}