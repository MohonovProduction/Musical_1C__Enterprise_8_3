using Storage;

namespace Presenter;

public interface ISoundPresenter
{
    // Добавление музыки
    Task<Sound> AddMusicAsync(string name, string author, CancellationToken token);

    // Удаление музыки
    Task DeleteMusicAsync(Sound sound, CancellationToken token);

    // Получение всех произведений музыки
    Task<IReadOnlyCollection<Sound>> GetMusicAsync(CancellationToken token);
}