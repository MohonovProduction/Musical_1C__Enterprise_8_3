namespace Storage;

public class Instrument
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Связь с музыкантами через таблицу musician_instrument
    public ICollection<MusicianInstrument> MusicianInstruments { get; set; }
    public Instrument() { }
}