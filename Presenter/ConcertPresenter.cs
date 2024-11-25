using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class ConcertPresenter : IConcertPresenter
    {
        private readonly MusicianOnConcertPresenter _musicianOnConcertPresenter = new MusicianOnConcertPresenter();
        private readonly SoundOnConcertPresenter _soundOnConcertPresenter = new SoundOnConcertPresenter();
        private readonly IConcertStorage _concertStorage; // Используем интерфейс для гибкости
        private ConcertBuilder _concertBuilder = new ConcertBuilder();

        public ConcertPresenter(IConcertStorage concertStorage)
        {
            _concertStorage = concertStorage ?? throw new ArgumentNullException(nameof(concertStorage));
        }

        // Добавление концерта
        public async Task AddConcertAsync(string name, CancellationToken token)
        {
            var fullConcert = _concertBuilder.BuildConcert(name);

            if (fullConcert != null)
            {
                var concert = new Concert(Guid.NewGuid(), fullConcert.Name, fullConcert.Type, fullConcert.Date);
                await _concertStorage.AddConcertAsync(concert, token);

                foreach (var music in fullConcert.Music)
                {
                    token.ThrowIfCancellationRequested();
                    await _soundOnConcertPresenter.AddSoundOnConcertAsync(concert.Id, music.Id, token);
                }

                foreach (var musician in fullConcert.Musicians)
                {
                    token.ThrowIfCancellationRequested();
                    await _musicianOnConcertPresenter.AddMusicianOnConcertAsync(concert.Id, musician.Id, token);
                }

                _concertBuilder = new ConcertBuilder(); // сброс builder после сохранения
            }
            else
            {
                Console.WriteLine("Ошибка: не все данные концерта заполнены.");
            }
        }

        // Удаление концерта
        public async Task DeleteConcertAsync(Concert concert, CancellationToken token)
        {
            if (concert == null)
            {
                throw new ArgumentNullException(nameof(concert));
            }

            await _concertStorage.DeleteConcertAsync(concert, token);
        }

        // Получение всех концертов
        public async Task<IReadOnlyCollection<Concert>> GetConcertsAsync(CancellationToken token)
        {
            return await _concertStorage.GetAllConcertsAsync(token);
        }

        // Установка типа концерта
        public void SetConcertType(string type)
        {
            _concertBuilder.Type = type;
        }

        // Добавление музыки в концерт
        public void AddMusicToConcert(Sound sound)
        {
            _concertBuilder.Music.Add(sound);
        }

        // Добавление музыканта в концерт
        public void AddMusicianToConcert(Musician musician)
        {
            _concertBuilder.Musicians.Add(musician);
        }

        // Установка даты концерта
        public void SetConcertDate(string date)
        {
            _concertBuilder.Date = date;
        }

        // Получить текущий ConcertBuilder
        public async Task<ConcertBuilder> GetConcertBuilderAsync()
        {
            return _concertBuilder;
        }
    }
}
