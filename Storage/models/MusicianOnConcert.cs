namespace Storage;

public class MusicianOnConcert
{
    public Guid ConcertId { get; set; }
    public Concert Concert { get; set; }

    public Guid MusicianId { get; set; }
    public Musician Musician { get; set; }

    // Конструктор для инициализации всех свойств
    public MusicianOnConcert(Guid concertId, Guid musicianId)
    {
        ConcertId = concertId;
        MusicianId = musicianId;
    }

    // Пустой конструктор для возможности создания объекта без инициализации
    public MusicianOnConcert()
    {
    }
}