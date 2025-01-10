using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentController : ControllerBase
    {
        private readonly IInstrumentPresenter _instrumentPresenter;

        // Constructor with dependency injection for IInstrumentPresenter
        public InstrumentController(IInstrumentPresenter instrumentPresenter)
        {
            _instrumentPresenter = instrumentPresenter;
        }

        // GET: api/instrument
        [HttpGet]
        public async Task<IActionResult> GetInstruments(CancellationToken cancellationToken)
        {
            try
            {
                var instruments = await _instrumentPresenter.GetInstrumentsAsync(cancellationToken);
                return Ok(instruments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/instrument/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstrumentById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var instrument = await _instrumentPresenter.GetInstrumentByIdAsync(id, cancellationToken);
                if (instrument == null)
                {
                    return NotFound();
                }

                return Ok(instrument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/instrument
        [HttpPost]
        public async Task<IActionResult> AddInstrument([FromBody] Instrument instrument, CancellationToken cancellationToken)
        {
            if (instrument == null || string.IsNullOrEmpty(instrument.Name))
            {
                return BadRequest("Invalid instrument data.");
            }

            try
            {
                // Using Guid.NewGuid() if instrument ID is empty
                await _instrumentPresenter.AddInstrumentAsync(instrument.Id == Guid.Empty ? Guid.NewGuid() : instrument.Id, instrument.Name, cancellationToken);
                return CreatedAtAction(nameof(GetInstrumentById), new { id = instrument.Id }, instrument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/instrument/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstrument(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var instrument = await _instrumentPresenter.GetInstrumentByIdAsync(id, cancellationToken);
                if (instrument == null)
                {
                    return NotFound();
                }

                await _instrumentPresenter.DeleteInstrumentAsync(instrument, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
