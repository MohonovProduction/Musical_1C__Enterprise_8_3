namespace Storage;

public interface IInstrumentStorage
{
    Task AddInstrumentAsync(Instrument instrument, CancellationToken token);
    Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token);
    Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token);
}