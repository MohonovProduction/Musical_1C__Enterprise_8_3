using Storage;

namespace Presenter;

public interface IConcertPresenter
{
    // Добавление концерта
    Task<bool> AddConcertAsync(string name, CancellationToken token);

    // Удаление концерта
    Task DeleteConcertAsync(Concert concert, CancellationToken token);

    // Получение всех концертов
    Task<IReadOnlyCollection<Concert>> GetConcertsAsync(CancellationToken token);

    // Установка типа концерта
    void SetConcertType(string type);

    // Добавление музыки в концерт
    void AddMusicToConcert(Sound sound);

    // Добавление музыканта в концерт
    void AddMusicianToConcert(Musician musician);

    // Установка даты концерта
    void SetConcertDate(string date);

    // Получить текущий ConcertBuilder
    Task<ConcertBuilder> GetConcertBuilderAsync();
}