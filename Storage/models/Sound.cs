namespace Storage
{
    public class Sound
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        // Связь с концертами через таблицу sound_on_concert
        public ICollection<SoundOnConcert> SoundOnConcerts { get; set; }

        // Конструктор для инициализации всех свойств
        public Sound(Guid id, string name, string author)
        {
            Id = id;
            Name = name;
            Author = author;
        }

        // Пустой конструктор для возможности создания объекта без инициализации
        public Sound()
        {
        }
    }
}