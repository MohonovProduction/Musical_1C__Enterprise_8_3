namespace Storage;

public class Instrument
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Instrument(Guid Id, string Name)
    {
        this.Id = Id;
        this.Name = Name;
    }
    public Instrument() { }
}