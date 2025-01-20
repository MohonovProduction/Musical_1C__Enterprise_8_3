namespace WebStation.Trains;

public class AddMusicianRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public List<InstrumentRequest> Instruments { get; set; }
    public List<ConcertRequest> Concerts { get; set; }
}