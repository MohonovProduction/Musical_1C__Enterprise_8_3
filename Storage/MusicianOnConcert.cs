namespace Storage;

public class MusicianOnConcert
{
    public Guid ConcertId { get; init; }
    public Guid MusicianId { get; init; }

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