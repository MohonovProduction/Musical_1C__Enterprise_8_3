namespace Storage;

public record Concert(Guid Id, string Name, string Type, List<Musician> Musicians,List<Music> Music, DateTime Date);