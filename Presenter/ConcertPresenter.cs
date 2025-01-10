using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Presenter
{
    public class ConcertPresenter : IConcertPresenter
    {
        private readonly IMusicianOnConcertPresenter _musicianOnConcertPresenter;
        private readonly ISoundOnConcertPresenter _soundOnConcertPresenter;
        private readonly IConcertStorage _concertStorage;
        private ConcertBuilder _concertBuilder;

        public ConcertPresenter(
            IConcertStorage concertStorage, 
            IMusicianOnConcertPresenter musicianOnConcertPresenter, 
            ISoundOnConcertPresenter soundOnConcertPresenter
        )
        {
            _concertStorage = concertStorage ?? throw new ArgumentNullException(nameof(concertStorage));
            _musicianOnConcertPresenter = musicianOnConcertPresenter ?? throw new ArgumentNullException(nameof(musicianOnConcertPresenter));
            _soundOnConcertPresenter = soundOnConcertPresenter ?? throw new ArgumentNullException(nameof(soundOnConcertPresenter));
            _concertBuilder = new ConcertBuilder();
        }

        // Добавление концерта
        public async Task<bool> AddConcertAsync(string name, CancellationToken token)
        {
            // Строим концерт с помощью ConcertBuilder
            var fullConcert = _concertBuilder.BuildConcert(name);

            // Проверяем, удалось ли создать концерт
            if (fullConcert != null)
            {
                try
                {
                    var concert = new Concert(Guid.NewGuid(), fullConcert.Name, fullConcert.Type, fullConcert.Date);
                    await _concertStorage.AddConcertAsync(concert, token);

                    // Добавляем произведения
                    foreach (var music in fullConcert.Music)
                    {
                        token.ThrowIfCancellationRequested();
                        await _soundOnConcertPresenter.AddSoundOnConcertAsync(concert.Id, music.Id, token);
                    }

                    // Добавляем музыкантов
                    foreach (var musician in fullConcert.Musicians)
                    {
                        token.ThrowIfCancellationRequested();
                        await _musicianOnConcertPresenter.AddMusicianOnConcertAsync(concert.Id, musician.Id, token);
                    }

                    _concertBuilder = new ConcertBuilder(); // Сбрасываем builder после сохранения

                    return true; // Возвращаем true в случае успешного выполнения
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении концерта: {ex.Message}");
                    return false; // Возвращаем false в случае ошибки
                }
            }
            else
            {
                Console.WriteLine("Ошибка: не все данные концерта заполнены.");
                return false; // Возвращаем false, если концерт не был построен
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
