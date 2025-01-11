using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IMusicianInstrumentPresenter
    {
        // Добавление инструмента музыканту
        Task AddMusicianInstrumentAsync(Guid musicianId, Guid instrumentId, CancellationToken token);

        // Удаление инструмента у музыканта
        Task DeleteMusicianInstrumentAsync(MusicianInstrument musicianInstrument, CancellationToken token);

        // Получение всех инструментов музыканта
        Task<IReadOnlyCollection<Instrument>> GetInstrumentsForMusicianAsync(Guid musicianId, CancellationToken token);

        // Получение всех музыкантов, играющих на инструменте
        Task<IReadOnlyCollection<Musician>> GetMusiciansForInstrumentAsync(Guid instrumentId, CancellationToken token);

        // Получение всех комбинаций музыкант-инструмент
        Task<IReadOnlyCollection<MusicianInstrument>> GetMusicianInstrumentsAsync(CancellationToken token);

        // Получение комбинации музыканта и инструмента по ID музыканта и инструмента
        Task<MusicianInstrument> GetMusicianInstrumentByIdsAsync(Guid musicianId, Guid instrumentId, CancellationToken token);
    }
}