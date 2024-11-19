﻿using Storage;

namespace Presenter;

public class InstrumentPresenter : IInstrumentPresenter
{
    private readonly IInstrumentStorage _instrumentStorage;

    public InstrumentPresenter(IInstrumentStorage instrumentStorage)
    {
        _instrumentStorage = instrumentStorage;
    }

    public InstrumentPresenter()
    {
        _instrumentStorage = new InstrumentStorage("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c", "instrument");
    }
    
    public async Task AddInstrumentAsync(Guid id, string name, CancellationToken token)
    {
        var instrument = new Instrument(id, name);
        await _instrumentStorage.AddInstrumentAsync(instrument, token);
    }

    public async Task DeleteInstrumentAsync(Instrument instrument, CancellationToken token)
    {
        await _instrumentStorage.DeleteInstrumentAsync(instrument, token);
    }

    public async Task<IReadOnlyCollection<Instrument>> GetInstrumentsAsync(CancellationToken token)
    {
       var instruments =  await _instrumentStorage.GetInstrumentsAsync(token);
       return instruments;
    }
}