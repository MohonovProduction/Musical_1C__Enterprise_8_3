using Storage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter;

public class MusicPresenter : IMusicPresenter
{
    private readonly MusicStorage _musicStorage = new MusicStorage("../../data/Music.json", "Music.json");

    public async Task<Music> AddMusicAsync(string name, string author, CancellationToken token)
    {
        var id = Guid.NewGuid();
        var music = new Music(id, name, author);
        await _musicStorage.AddMusicAsync(music, token);
        return music; // Возвращаем добавленное произведение
    }

    public async Task DeleteMusicAsync(Music music, CancellationToken token)
    {
        await _musicStorage.DeleteMusicAsync(music, token);
    }

    public async Task<IReadOnlyCollection<Music>> GetMusicAsync(CancellationToken token)
    {
        var music = await _musicStorage.GetAllMusicAsync(token);
        return music;
    }
}