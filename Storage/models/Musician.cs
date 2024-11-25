namespace Storage 
{
    public class Musician
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Surname { get; set; }

        // Связь с концертами через таблицу musician_on_concert
        public ICollection<MusicianOnConcert> MusicianOnConcerts { get; set; }

        // Связь с инструментами через таблицу musician_instrument
        public ICollection<MusicianInstrument> MusicianInstruments { get; set; }

        // Пустой конструктор для возможности создания объекта без инициализации
        public Musician()
        {
        }
    }
}