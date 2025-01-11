// using Microsoft.AspNetCore.Mvc;
// using Presenter;
// using Storage;
// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace WebApi.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class SoundsController : ControllerBase
//     {
//         private readonly ISoundPresenter _soundPresenter;
//
//         public SoundsController(ISoundPresenter soundPresenter)
//         {
//             _soundPresenter = soundPresenter;
//         }
//
//         // GET: api/Sounds
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Sound>>> GetSounds(CancellationToken token)
//         {
//             try
//             {
//                 var sounds = await _soundPresenter.GetMusicAsync(token);
//                 return Ok(sounds);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }
//
//         // GET: api/Sounds/{id}
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Sound>> GetSoundById(Guid id, CancellationToken token)
//         {
//             try
//             {
//                 var sound = await _soundPresenter.GetMusicByIdAsync(id, token);
//
//                 if (sound == null)
//                 {
//                     return NotFound();
//                 }
//
//                 return Ok(sound);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }
//
//         // POST: api/Sounds
//         [HttpPost]
//         public async Task<ActionResult> AddSound([FromBody] SoundCreateRequest request, CancellationToken token)
//         {
//             try
//             {
//                 var sound = await _soundPresenter.AddMusicAsync(request.Name, request.Author, token);
//                 return CreatedAtAction(nameof(GetSoundById), new { id = sound.Id }, null);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }
//
//         // DELETE: api/Sounds/{id}
//         [HttpDelete("{id}")]
//         public async Task<ActionResult> DeleteSound(Guid id, CancellationToken token)
//         {
//             try
//             {
//                 var sound = await _soundPresenter.GetMusicByIdAsync(id, token);
//                 if (sound == null)
//                 {
//                     return NotFound();
//                 }
//
//                 await _soundPresenter.DeleteMusicAsync(id, token);
//                 return NoContent();
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }
//     }
//
//     public class SoundCreateRequest
//     {
//         public string Name { get; set; }
//         public string Author { get; set; }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundsController : ControllerBase
    {
        private readonly ISoundPresenter _soundPresenter;
        private readonly ISoundOnConcertPresenter _soundOnConcertPresenter;

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
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Sounds
        [HttpPost]
        public async Task<ActionResult> AddSound([FromBody] SoundCreateRequest request, CancellationToken token)
        {
            try
            {
                var sound = await _soundPresenter.AddMusicAsync(request.Name, request.Author, token);
                return CreatedAtAction(nameof(GetSoundById), new { id = sound.Id }, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class SoundCreateRequest
    {
        public string Name { get; set; }
        public string Author { get; set; }
    }
}
