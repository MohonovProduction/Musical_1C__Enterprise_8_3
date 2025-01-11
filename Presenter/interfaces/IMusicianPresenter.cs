using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IMusicianPresenter
    {
        // Добавление музыканта
        Task<Musician> AddMusicianAsync(string name, string lastName, string surname, 
            List<Instrument> instruments, List<Concert> concerts, CancellationToken token);

        // Удаление музыканта
        Task DeleteMusicianAsync(Musician musician, CancellationToken token);

        // Получение всех музыкантов
        Task<IReadOnlyCollection<Musician>> GetMusiciansAsync(CancellationToken token);

        // Получение музыканта по ID
        Task<Musician> GetMusicianByIdAsync(Guid id, CancellationToken token);
    }
}