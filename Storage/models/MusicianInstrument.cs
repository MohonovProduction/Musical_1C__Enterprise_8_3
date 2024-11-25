namespace Storage
{
    public class MusicianInstrument
    {
        public Guid MusicianId { get; set; }
        public Musician Musician { get; set; }
        public Guid InstrumentId { get; set; }
        public Instrument Instrument { get; set; }

        // Конструктор для инициализации всех свойств
        public MusicianInstrument(Guid musicianId, Guid instrumentId, Musician musician, Instrument instrument)
        {
            MusicianId = musicianId;
            InstrumentId = instrumentId;
            Musician = musician;
            Instrument = instrument;
        }

        // Пустой конструктор для возможности создания объекта без инициализации
        public MusicianInstrument(Guid musicianId, Guid instrumentId)
        {
            MusicianId = musicianId;
            InstrumentId = instrumentId;
        }

        public MusicianInstrument()
        {
        }
    }
}