using Storage;

namespace Presenter;

public interface IInstrumentPresenter
{
    Task AddInstrumentAsync(Guid id, string name, CancellationToken token);
    Task DeleteInstrumentAsync(Instrument instrument , CancellationToken token);
    Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token);
    
}