using System;
using System.Collections.Generic;
using Storage;

namespace Presenter;

public class ConcertBuilder
{
    public string Type { get; set; }
    public List<Musician> Musicians { get; } = new();
    public List<Music> Music { get; } = new();
    public DateTime Date { get; set; }

    public Concert BuildConcert(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(Type) || Date == default)
        {
            return null; // Вернуть null, если не заполнены обязательные поля
        }

        return new Concert(Guid.NewGuid(), name, Type, Musicians, Music, Date);
    }
}