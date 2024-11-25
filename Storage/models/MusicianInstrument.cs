namespace Storage
{
    public class MusicianInstrument
    {
        public Guid MusicianId { get; set; }
        public Musician Musician { get; set; }

        public Guid InstrumentId { get; set; }
        public Instrument Instrument { get; set; }

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