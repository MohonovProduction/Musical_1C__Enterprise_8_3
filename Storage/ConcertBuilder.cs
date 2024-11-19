using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Storage;

namespace Presenter;

public class ConcertBuilder
{
    public string Name { get; set; }
    public string Type { get; set; }
    public List<Musician> Musicians { get; } = new();
    public List<Music> Music { get; } = new();
    public string Date { get; set; }

    public ConcertBuilder()
    {
        
    }

    private ConcertBuilder(string name, string type, List<Musician> musicians, List<Music> music, string date)
    {
        Name = name;
        Type = type;
        Musicians = musicians;
        Music = music;
        Date = date;
    }

    public ConcertBuilder BuildConcert(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(Type) || Date == default)
        {
            return null; // Вернуть null, если не заполнены обязательные поля
        }

        return new ConcertBuilder(name, this.Type, this.Musicians, this.Music, this.Date);
    }
}