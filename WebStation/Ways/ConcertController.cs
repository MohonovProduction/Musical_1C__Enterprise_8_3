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
    public class ConcertsController : ControllerBase
    {
        private readonly IConcertPresenter _concertPresenter;
        private readonly ISoundPresenter _soundPresenter;
        private readonly IMusicianPresenter _musicianPresenter;

        public ConcertsController(IConcertPresenter concertPresenter)
        {
            _concertPresenter = concertPresenter;
        }

        // GET: api/Concerts
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Concert>>> GetConcerts(CancellationToken token)
        {
            try
            {
                var concerts = await _concertPresenter.GetConcertsAsync(token);
                return Ok(concerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Concerts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Concert>> GetConcertById(Guid id, CancellationToken token)
        {
            try
            {
                var concert = await _concertPresenter.GetConcertByIdAsync(id, token);

                if (concert == null)
                {
                    return NotFound();
                }

                return Ok(concert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Concerts
        [HttpPost]
        public async Task<ActionResult> AddConcert([FromBody] ConcertRequest request, CancellationToken token)
        {
            try
            {
                // Логика для получения музыкантов из запроса
                var musicians = request.Musicians.Select(m => new Musician { Id = m.Id }).ToList();

                // Логика для получения произведений из запроса
                var sounds = request.Sounds.Select(s => new Sound { Id = s.Id }).ToList();

                var concert = await _concertPresenter.AddConcertAsync(
                    request.Name, 
                    request.Type, 
                    request.Date, 
                    musicians, 
                    sounds, 
                    token
                );

                return CreatedAtAction(nameof(GetConcertById), new { id = concert.Id }, concert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Concerts/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConcert(Guid id, CancellationToken token)
        {
            try
            {
                var concert = await _concertPresenter.GetConcertByIdAsync(id, token);
                if (concert == null)
                {
                    return NotFound();
                }

                await _concertPresenter.DeleteConcertAsync(concert, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Concerts/{concertId}/Musicians/{musicianId}
        [HttpPost("{concertId}/Musicians/{musicianId}")]
        public async Task<ActionResult> AddMusicianToConcert(Guid concertId, Guid musicianId, CancellationToken token)
        {
            try
            {
                var concert = await _concertPresenter.GetConcertByIdAsync(concertId, token);
                var musician = await _musicianPresenter.GetMusicianByIdAsync(musicianId, token);

                if (concert == null || musician == null)
                {
                    return NotFound();
                }

                await _concertPresenter.AddMusicianToConcertAsync(concert, musician, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Concerts/{concertId}/Musicians/{musicianId}
        [HttpDelete("{concertId}/Musicians/{musicianId}")]
        public async Task<ActionResult> RemoveMusicianFromConcert(Guid concertId, Guid musicianId, CancellationToken token)
        {
            try
            {
                var concert = await _concertPresenter.GetConcertByIdAsync(concertId, token);
                var musician = await _musicianPresenter.GetMusicianByIdAsync(musicianId, token);

                if (concert == null || musician == null)
                {
                    return NotFound();
                }

                await _concertPresenter.RemoveMusicianFromConcertAsync(concert, musician, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Concerts/{concertId}/Sounds/{soundId}
        [HttpPost("{concertId}/Sounds/{soundId}")]
        public async Task<ActionResult> AddSoundToConcert(Guid concertId, Guid soundId, CancellationToken token)
        {
            try
            {
                var concert = await _concertPresenter.GetConcertByIdAsync(concertId, token);
                var sound = await _soundPresenter.GetMusicByIdAsync(soundId, token); // Assume a SoundPresenter exists

                if (concert == null || sound == null)
                {
                    return NotFound();
                }

                await _concertPresenter.AddSoundToConcertAsync(concert, sound, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Concerts/{concertId}/Sounds/{soundId}
        [HttpDelete("{concertId}/Sounds/{soundId}")]
        public async Task<ActionResult> RemoveSoundFromConcert(Guid concertId, Guid soundId, CancellationToken token)
        {
            try
            {
                var concert = await _concertPresenter.GetConcertByIdAsync(concertId, token);
                var sound = await _soundPresenter.GetMusicByIdAsync(soundId, token); // Assume a SoundPresenter exists

                if (concert == null || sound == null)
                {
                    return NotFound();
                }

                await _concertPresenter.RemoveSoundFromConcertAsync(concert, sound, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class ConcertRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public List<MusicianIdRequest> Musicians { get; set; }
        public List<SoundIdRequest> Sounds { get; set; }
    }

    public class MusicianIdRequest
    {
        public Guid Id { get; set; }
    }

    public class SoundIdRequest
    {
        public Guid Id { get; set; }
    }
}
