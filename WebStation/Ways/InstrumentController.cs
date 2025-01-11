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
    public class InstrumentsController : ControllerBase
    {
        private readonly IInstrumentPresenter _instrumentPresenter;

        public InstrumentsController(IInstrumentPresenter instrumentPresenter)
        {
            _instrumentPresenter = instrumentPresenter;
        }

        // GET: api/Instruments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instrument>>> GetInstruments(CancellationToken token)
        {
            try
            {
                var instruments = await _instrumentPresenter.GetInstrumentsAsync(token);
                return Ok(instruments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Instruments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Instrument>> GetInstrumentById(Guid id, CancellationToken token)
        {
            try
            {
                var instrument = await _instrumentPresenter.GetInstrumentByIdAsync(id, token);

                if (instrument == null)
                {
                    return NotFound();
                }

                return Ok(instrument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Instruments
        [HttpPost]
        public async Task<ActionResult> AddInstrument(Guid id, [FromBody] string name, CancellationToken token)
        {
            try
            {
                await _instrumentPresenter.AddInstrumentAsync(id, name, token);
                return CreatedAtAction(nameof(GetInstrumentById), new { id }, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Instruments/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInstrument(Guid id, CancellationToken token)
        {
            try
            {
                var instrument = await _instrumentPresenter.GetInstrumentByIdAsync(id, token);
                if (instrument == null)
                {
                    return NotFound();
                }

                await _instrumentPresenter.DeleteInstrumentAsync(instrument, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
