using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianController : ControllerBase
    {
        private readonly IMusicianPresenter _musicianPresenter;

        public MusicianController(IMusicianPresenter musicianPresenter)
        {
            _musicianPresenter = musicianPresenter ?? throw new ArgumentNullException(nameof(musicianPresenter));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMusicianAsync([FromBody] AddMusicianRequest request, CancellationToken token)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Surname))
            {
                return BadRequest("Name, LastName, and Surname are required.");
            }

            var instruments = request.Instruments?.Select(i => new Instrument(Guid.NewGuid(), i.Name)).ToList() ?? new List<Instrument>();

            var musician = await _musicianPresenter.AddMusicianAsync(request.Name, request.LastName, request.Surname, instruments, token);
            return Ok(musician);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMusicianAsync(Guid id, CancellationToken token)
        {
            var musicians = await _musicianPresenter.GetMusiciansAsync(token);
            var musician = musicians.FirstOrDefault(m => m.Id == id);

            if (musician == null)
            {
                return NotFound("Musician not found.");
            }

            await _musicianPresenter.DeleteMusicianAsync(musician, token);
            return Ok("Musician deleted successfully.");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetMusiciansAsync(CancellationToken token)
        {
            var musicians = await _musicianPresenter.GetMusiciansAsync(token);
            return Ok(musicians);
        }
    }

    public class AddMusicianRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public List<InstrumentRequest> Instruments { get; set; }
    }

    public class InstrumentRequest
    {
        public string Name { get; set; }
    }
}
