using WebStation.Ways;

namespace WebStation.Trains;

public class ConcertResponse
{
    public Guid Id { get; set; } // Id концерта
    public string Name { get; set; } // Название концерта
    public string Type { get; set; } // Тип концерта
    public string Date { get; set; } // Дата концерта
    public List<MusicianResponse> Musicians { get; set; } // Список музыкантов
    public List<SoundResponse> Sounds { get; set; } // Список произведений
}