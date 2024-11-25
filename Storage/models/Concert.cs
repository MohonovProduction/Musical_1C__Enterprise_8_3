namespace Storage
{
    public class Concert
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }

        // Связь с музыкантами через таблицу musician_on_concert
        public ICollection<MusicianOnConcert> MusicianOnConcerts { get; set; }

        // Связь с произведениями через таблицу sound_on_concert
        public ICollection<SoundOnConcert> SoundOnConcerts { get; set; }

        // Конструктор для инициализации всех свойств
        public Concert(Guid id, string name, string type, string date)
        {
            Id = id;
            Name = name;
            Type = type;
            Date = date;
        }

        // Пустой конструктор для возможности создания объекта без инициализации
        public Concert()
        {
        }
    }
}