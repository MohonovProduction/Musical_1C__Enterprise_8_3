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
    public class MusicianInstrumentController : ControllerBase
    {
        private readonly MusicianInstrumentPresenter _presenter;

        public MusicianInstrumentController(MusicianInstrumentPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddMusicianInstrument(Guid musicianId, Guid instrumentId, CancellationToken cancellationToken)
        {
            if (musicianId == Guid.Empty || instrumentId == Guid.Empty)
                return BadRequest("MusicianId and InstrumentId must be valid GUIDs.");

            await _presenter.AddMusicianInstrumentAsync(musicianId, instrumentId, cancellationToken);
            return Ok("Musician-Instrument relationship added successfully.");
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetMusicianInstruments(CancellationToken cancellationToken)
        {
            var musicianInstruments = await _presenter.GetMusicianInstrumentAsync(cancellationToken);
            return Ok(musicianInstruments);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteMusicianInstrument(Guid musicianId, Guid instrumentId, CancellationToken cancellationToken)
        {
            if (musicianId == Guid.Empty || instrumentId == Guid.Empty)
                return BadRequest("MusicianId and InstrumentId must be valid GUIDs.");

            await _presenter.DeleteMusicianInstrumentAsync(musicianId, instrumentId, cancellationToken);
            return Ok("Musician-Instrument relationship deleted successfully.");
        }
    }
}