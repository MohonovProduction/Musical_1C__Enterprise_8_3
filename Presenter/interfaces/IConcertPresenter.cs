using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IConcertPresenter
    {
        // Добавление концерта
        Task<Concert> AddConcertAsync(string name, string type, string date, 
            List<Musician> musicians, List<Sound> sounds, CancellationToken token);

        // Удаление концерта
        Task DeleteConcertAsync(Concert concert, CancellationToken token);

        // Получение всех концертов
        Task<IReadOnlyCollection<Concert>> GetConcertsAsync(CancellationToken token);

        // Получение концерта по ID
        Task<Concert> GetConcertByIdAsync(Guid id, CancellationToken token);

        // Добавление музыканта на концерт
        Task AddMusicianToConcertAsync(Concert concert, Musician musician, CancellationToken token);

        // Удаление музыканта с концерта
        Task RemoveMusicianFromConcertAsync(Concert concert, Musician musician, CancellationToken token);

        // Добавление произведения (звука) на концерт
        Task AddSoundToConcertAsync(Concert concert, Sound sound, CancellationToken token);

        // Удаление произведения (звука) с концерта
        Task RemoveSoundFromConcertAsync(Concert concert, Sound sound, CancellationToken token);
    }
}