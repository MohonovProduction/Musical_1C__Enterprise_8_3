using WebApi.Controllers;
using WebAPI.Trains;

public record ConcertRequest
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Date { get; set; }
    public List<MusicianIdRequest> Musicians { get; set; }
    public List<SoundIdRequest> Sounds { get; set; }
}