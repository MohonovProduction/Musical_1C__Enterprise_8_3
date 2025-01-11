using Storage;

namespace WebAPI.Trains;

public record MusicianRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public List<Instrument> Instruments { get; set; }
    public List<Concert> Concerts { get; set; }
}