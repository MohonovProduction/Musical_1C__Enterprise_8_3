using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter;

public class ConcertPresenter : IConcertPresenter
{
    private readonly ConcertStorage _concertStorage = new ConcertStorage("../../data/Concerts.json", "Concerts");
    private ConcertBuilder _concertBuilder = new ConcertBuilder();

    public async Task AddConcertAsync(string name,CancellationToken token)
    {
        var concert = _concertBuilder.BuildConcert(name);
        if (concert != null)
        {
            await _concertStorage.AddConcertAsync(concert, token);
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
        return await _concertStorage.GetAllConcerts(token);
    }

    public void SetConcertType(string type)
    {
        _concertBuilder.Type = type;
    }

    public void AddMusicToConcert(Music music)
    {
        _concertBuilder.Music.Add(music);
    }

    public void AddMusicianToConcert(Musician musician)
    {
        _concertBuilder.Musicians.Add(musician);
    }

    public void SetConcertDate(DateTime date)
    {
        _concertBuilder.Date = date;
    }

    public async Task<ConcertBuilder> GetConcertBuilderAsync()
    {
        return _concertBuilder;
    }
   
}