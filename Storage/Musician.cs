namespace Storage 
{
    public class Musician
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string LastName { get; init; }
        public string Surname { get; init; }
        //public List<Instrument> Instruments { get; init; } TODO: remove

        // Конструктор для инициализации всех свойств
        public Musician(Guid id, string name, string lastName, string surname)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Surname = surname;
            //Instruments = instruments; TODO: remove
        }

        // Пустой конструктор для возможности создания объекта без инициализации
        public Musician()
        {
        }
    }
}