using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Trains;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianController : ControllerBase
    {
        private readonly IMusicianPresenter _musicianPresenter;

        // Конструктор: внедрение зависимостей через DI контейнер
        public MusicianController(IMusicianPresenter musicianPresenter)
        {
            _musicianPresenter = musicianPresenter;
        }

        // Получение всех музыкантов с инструментами и концертами
        [HttpGet]
        public async Task<IActionResult> GetMusicians(CancellationToken token)
        {
            try
            {
                var musicians = await _musicianPresenter.GetMusiciansAsync(token);
                return Ok(musicians);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Получение музыканта по ID с инструментами и концертами
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicianById(Guid id, CancellationToken token)
        {
            try
            {
                var musician = await _musicianPresenter.GetMusicianByIdAsync(id, token);
                if (musician == null)
                {
                    return NotFound($"Musician with ID {id} not found.");
                }

                return Ok(musician);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Добавление музыканта с инструментами и концертами
        [HttpPost]
        public async Task<IActionResult> AddMusician([FromBody] MusicianRequest model, CancellationToken token)
        {
            try
            {
                if (model == null || model.Instruments == null || model.Concerts == null)
                {
                    return BadRequest("Invalid input data.");
                }

                var musician = await _musicianPresenter.AddMusicianAsync(
                    model.Name,
                    model.LastName,
                    model.Surname,
                    model.Instruments,
                    model.Concerts,
                    token
                );

                return CreatedAtAction(nameof(GetMusicianById), new { id = musician.Id }, musician);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Удаление музыканта и его связей с инструментами
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusician(Guid id, CancellationToken token)
        {
            try
            {
                var musician = await _musicianPresenter.GetMusicianByIdAsync(id, token);
                if (musician == null)
                {
                    return NotFound($"Musician with ID {id} not found.");
                }

                await _musicianPresenter.DeleteMusicianAsync(musician, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
