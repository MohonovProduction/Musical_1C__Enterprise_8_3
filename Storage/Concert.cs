namespace Storage
{
    public class Concert
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }
        public string Date { get; init; }

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