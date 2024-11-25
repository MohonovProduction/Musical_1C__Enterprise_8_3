﻿using Storage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter;

public class MusicPresenter : IMusicPresenter
{
    private readonly IMusicStorage _musicStorage;
    
    public MusicPresenter(IMusicStorage musicStorage)
    {
        _musicStorage = musicStorage;
    }
    public MusicPresenter()
    {
        _musicStorage = new MusicStorage("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c", "sound");
    }
    
    public async Task<Sound> AddMusicAsync(string name, string author, CancellationToken token)
    {
        var id = Guid.NewGuid();
        var music = new Sound(id, name, author);
        await _musicStorage.AddMusicAsync(music, token);
        return music; // Возвращаем добавленное произведение
    }

    public async Task DeleteMusicAsync(Sound sound, CancellationToken token)
    {
        await _musicStorage.DeleteMusicAsync(sound, token);
    }

    public async Task<IReadOnlyCollection<Sound>> GetMusicAsync(CancellationToken token)
    {
        var music = await _musicStorage.GetAllMusicAsync(token);
        return music;
    }
}