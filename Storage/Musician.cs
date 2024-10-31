namespace Storage;

public record Musician(Guid Id, string Name, string LastName, string Surname, List<Instrument> Instruments);