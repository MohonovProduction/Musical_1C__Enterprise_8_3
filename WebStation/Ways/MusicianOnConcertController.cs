using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianOnConcertController : ControllerBase
    {
        private readonly MusicianOnConcertPresenter _musicianOnConcertPresenter;

        public MusicianOnConcertController(MusicianOnConcertPresenter musicianOnConcertPresenter)
        {
            _musicianOnConcertPresenter = musicianOnConcertPresenter;
        }

        // DTO for adding a musician to a concert
        public class MusicianOnConcertDto
        {
            public Guid ConcertId { get; set; }
            public Guid MusicianId { get; set; }
        }

        // POST: api/MusicianOnConcert
        [HttpPost]
        public async Task<IActionResult> AddMusicianOnConcert([FromBody] MusicianOnConcertDto dto, CancellationToken token)
        {
            if (dto.ConcertId == Guid.Empty || dto.MusicianId == Guid.Empty)
            {
                return BadRequest("ConcertId and MusicianId cannot be empty.");
            }

            try
            {
                await _musicianOnConcertPresenter.AddMusicianOnConcertAsync(dto.ConcertId, dto.MusicianId, token);
                return Ok("Musician successfully added to the concert.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET: api/MusicianOnConcert
        [HttpGet]
        public async Task<IActionResult> GetAllMusiciansOnConcert(CancellationToken token)
        {
            try
            {
                var results = await _musicianOnConcertPresenter.GetMusicianOnConcertAsync(token);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // DELETE: api/MusicianOnConcert
        [HttpDelete]
        public async Task<IActionResult> DeleteMusicianOnConcert([FromQuery] Guid concertId, [FromQuery] Guid musicianId, CancellationToken token)
        {
            if (concertId == Guid.Empty || musicianId == Guid.Empty)
            {
                return BadRequest("ConcertId and MusicianId cannot be empty.");
            }

            try
            {
                await _musicianOnConcertPresenter.DeleteMusicianOnConcertAsync(concertId, musicianId, token);
                return Ok("Musician successfully removed from the concert.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}