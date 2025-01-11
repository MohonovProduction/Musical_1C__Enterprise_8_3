using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IMusicianOnConcertPresenter
    {
        // Добавление музыканта на концерт
        Task AddMusicianOnConcertAsync(Guid concertId, Guid musicianId, CancellationToken token);

        // Удаление музыканта с концерта
        Task DeleteMusicianOnConcertAsync(MusicianOnConcert musicianOnConcert, CancellationToken token);

        // Получение всех музыкантов на концерте
        Task<IReadOnlyCollection<Musician>> GetMusiciansOnConcertAsync(Guid concertId, CancellationToken token);

        // Получение всех концертов музыканта
        Task<IReadOnlyCollection<Concert>> GetConcertsForMusicianAsync(Guid musicianId, CancellationToken token);

        // Получение всех музыкантов на концертах
        Task<IReadOnlyCollection<MusicianOnConcert>> GetMusiciansOnConcertsAsync(CancellationToken token);

        // Получение музыканта на концерте по ID концерта и музыканта
        Task<MusicianOnConcert> GetMusicianOnConcertByIdsAsync(Guid concertId, Guid musicianId, CancellationToken token);
    }
}