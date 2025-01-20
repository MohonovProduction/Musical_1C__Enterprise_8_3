using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebStation.Trains;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundController : ControllerBase
    {
        private readonly ISoundPresenter _soundPresenter;

        public SoundController(ISoundPresenter soundPresenter)
        {
            _soundPresenter = soundPresenter;
        }

        // Добавление нового произведения музыки
        [HttpPost]
        public async Task<IActionResult> AddMusic([FromBody] AddSoundRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Invalid music data.");
            }

            try
            {
                var sound = await _soundPresenter.AddMusicAsync(request.Name, request.Author, cancellationToken);
                return CreatedAtAction(nameof(GetMusicById), new { id = sound.Id }, request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Удаление произведения музыки
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusic(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var sound = await _soundPresenter.GetMusicByIdAsync(id, cancellationToken);
                if (sound == null)
                {
                    return NotFound($"Music with ID {id} not found.");
                }

                await _soundPresenter.DeleteMusicAsync(id, cancellationToken);
                return NoContent(); // Успешное удаление
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Получение списка всех произведений музыки
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetAllMusic(CancellationToken cancellationToken)
        {
            try
            {
                var musicList = await _soundPresenter.GetMusicAsync(cancellationToken);
                return Ok(musicList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Получение произведения музыки по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Sound>> GetMusicById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var sound = await _soundPresenter.GetMusicByIdAsync(id, cancellationToken);
                if (sound == null)
                {
                    return NotFound($"Music with ID {id} not found.");
                }

                return Ok(sound);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
