using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public interface ISoundPresenter
    {
        // Добавление произведения музыки
        Task<Sound> AddMusicAsync(string name, string author, CancellationToken token);

        // Удаление произведения музыки
        Task DeleteMusicAsync(Guid soundId, CancellationToken token);

        // Получение всех произведений музыки
        Task<IReadOnlyCollection<Sound>> GetMusicAsync(CancellationToken token);

        // Получение произведения музыки по ID
        Task<Sound> GetMusicByIdAsync(Guid soundId, CancellationToken token);
    }
}