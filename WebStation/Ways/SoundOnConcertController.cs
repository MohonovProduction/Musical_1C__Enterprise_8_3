using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoundOnConcertController : ControllerBase
    {
        private readonly SoundOnConcertPresenter _presenter;

        public SoundOnConcertController(SoundOnConcertPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        // POST: api/SoundOnConcert
        [HttpPost]
        public async Task<IActionResult> AddSoundOnConcert([FromBody] SoundOnConcertDto soundOnConcertDto, CancellationToken cancellationToken)
        {
            if (soundOnConcertDto == null)
                return BadRequest("Invalid data.");

            try
            {
                await _presenter.AddSoundOnConcertAsync(soundOnConcertDto.ConcertId, soundOnConcertDto.SoundId, cancellationToken);
                return Ok("Sound linked to concert successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SoundOnConcert
        [HttpGet]
        public async Task<IActionResult> GetAllSoundOnConcert(CancellationToken cancellationToken)
        {
            try
            {
                var soundOnConcerts = await _presenter.GetSoundOnConcertAsync(cancellationToken);
                return Ok(soundOnConcerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    // DTO class to represent input data for creating SoundOnConcert
    public class SoundOnConcertDto
    {
        public Guid ConcertId { get; set; }
        public Guid SoundId { get; set; }
    }
}