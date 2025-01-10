using Storage;

namespace Presenter;

public interface IInstrumentPresenter
{
    // Добавление инструмента
    Task AddInstrumentAsync(Guid id, string name, CancellationToken token);

    // Удаление инструмента
    Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token);

    // Получение всех инструментов
    Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token);

    // Получение инструмента по ID
    Task<Instrument> GetInstrumentByIdAsync(Guid id, CancellationToken token);
}