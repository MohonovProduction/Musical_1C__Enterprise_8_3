namespace Storage
{
    public class MusicianInstrument
    {
        public Guid MusicianId { get; init; }
        public Guid InstrumentId { get; init; }

        // Конструктор для инициализации всех свойств
        public MusicianInstrument(Guid musicianId, Guid instrumentId)
        {
            MusicianId = musicianId;
            InstrumentId = instrumentId;
        }

        // Пустой конструктор для возможности создания объекта без инициализации
        public MusicianInstrument()
        {
        }
    }
}