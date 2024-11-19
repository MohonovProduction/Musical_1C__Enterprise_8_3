namespace Storage
{
    public class Music
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Author { get; init; }

        // Конструктор для инициализации всех свойств
        public Music(Guid id, string name, string author)
        {
            Id = id;
            Name = name;
            Author = author;
        }

        // Пустой конструктор для возможности создания объекта без инициализации
        public Music()
        {
        }
    }
}