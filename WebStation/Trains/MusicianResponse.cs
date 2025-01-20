namespace WebStation.Trains;

public class MusicianResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public List<InstrumentResponse> Instruments { get; set; }
    public List<ConcertResponse> Concerts { get; set; }
}