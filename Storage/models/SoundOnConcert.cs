namespace Storage;

public class SoundOnConcert
{
    public Guid ConcertId { get; init; }
    public Guid SoundId { get; init; }

    // Конструктор для инициализации всех свойств
    public SoundOnConcert(Guid concertId, Guid soundId)
    {
        ConcertId = concertId;
        SoundId = soundId;
    }

    // Пустой конструктор для возможности создания объекта без инициализации
    public SoundOnConcert()
    {
    }
}