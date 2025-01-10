using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundController : ControllerBase
    {
        private readonly ISoundPresenter _soundPresenter;

        public SoundController(ISoundPresenter soundPresenter)
        {
            _soundPresenter = soundPresenter ?? throw new ArgumentNullException(nameof(soundPresenter));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMusicAsync([FromBody] AddSoundRequest request, CancellationToken token)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Author))
            {
                return BadRequest("Name and Author are required.");
            }

            var sound = await _soundPresenter.AddMusicAsync(request.Name, request.Author, token);
            return Ok(sound);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMusicAsync(Guid id, CancellationToken token)
        {
            var sounds = await _soundPresenter.GetMusicAsync(token);
            var sound = sounds.FirstOrDefault(s => s.Id == id);

            if (sound == null)
            {
                return NotFound("Sound not found.");
            }

            await _soundPresenter.DeleteMusicAsync(sound, token);
            return Ok("Sound deleted successfully.");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetMusicAsync(CancellationToken token)
        {
            var sounds = await _soundPresenter.GetMusicAsync(token);
            return Ok(sounds);
        }
    }

    public class AddSoundRequest
    {
        public string Name { get; set; }
        public string Author { get; set; }
    }
}