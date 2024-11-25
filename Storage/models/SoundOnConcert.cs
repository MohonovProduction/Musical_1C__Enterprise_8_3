namespace Storage;

public class SoundOnConcert
{
    public Guid ConcertId { get; set; }
    public Concert Concert { get; set; }

    public Guid SoundId { get; set; }
    public Sound Sound { get; set; }

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