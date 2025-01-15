using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Trains;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundsController : ControllerBase
    {
        private readonly ISoundPresenter _soundPresenter;
        private readonly ISoundOnConcertPresenter _soundOnConcertPresenter;

        // Конструктор: внедрение зависимостей через DI контейнер
        public SoundsController(ISoundPresenter soundPresenter, ISoundOnConcertPresenter soundOnConcertPresenter)
        {
            _soundPresenter = soundPresenter;
            _soundOnConcertPresenter = soundOnConcertPresenter;
        }

        // GET: api/Sounds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetSounds(CancellationToken token)
        {
            try
            {
                // Получаем список всех звуков
                var sounds = await _soundPresenter.GetMusicAsync(token);

                // Для каждого звука добавляем информацию о SoundOnConcert
                foreach (var sound in sounds)
                {
                    var soundOnConcerts = await _soundOnConcertPresenter.GetSoundOnConcertAsync(token);
                    sound.SoundOnConcerts = soundOnConcerts.Where(soc => soc.SoundId == sound.Id).ToList();
                }

                return Ok(sounds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Sounds/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Sound>> GetSoundById(Guid id, CancellationToken token)
        {
            try
            {
                var sound = await _soundPresenter.GetMusicByIdAsync(id, token);

                if (sound == null)
                {
                    return NotFound();
                }

                // Добавляем информацию о SoundOnConcert для конкретного звука
                var soundOnConcerts = await _soundOnConcertPresenter.GetSoundOnConcertAsync(token);
                sound.SoundOnConcerts = soundOnConcerts.Where(soc => soc.SoundId == sound.Id).ToList();

                return Ok(sound);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Sounds
        [HttpPost]
        public async Task<ActionResult> AddSound([FromBody] SoundRequest request, CancellationToken token)
        {
            try
            {
                var sound = await _soundPresenter.AddMusicAsync(request.Name, request.Author, token);
                return CreatedAtAction(nameof(GetSoundById), new { id = sound.Id }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Sounds/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSound(Guid id, CancellationToken token)
        {
            try
            {
                var sound = await _soundPresenter.GetMusicByIdAsync(id, token);
                if (sound == null)
                {
                    return NotFound();
                }

                await _soundPresenter.DeleteMusicAsync(id, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
