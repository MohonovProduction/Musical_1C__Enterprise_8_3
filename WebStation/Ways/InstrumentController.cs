using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using WebStation.Trains;

namespace WebStation.Ways
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly IInstrumentPresenter _instrumentPresenter;

        public InstrumentController(IInstrumentPresenter instrumentPresenter)
        {
            _instrumentPresenter = instrumentPresenter;
        }

        // Добавление нового инструмента
        [HttpPost]
        public async Task<IActionResult> AddInstrument([FromBody] AddInstrumentRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Invalid instrument data.");
            }

            var instrumentId = Guid.NewGuid(); // Генерация уникального идентификатора
            try
            {
                await _instrumentPresenter.AddInstrumentAsync(instrumentId, request.Name, cancellationToken);
                return CreatedAtAction(nameof(GetInstrumentById), new { id = instrumentId }, request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Удаление инструмента
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstrument(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var instrument = await _instrumentPresenter.GetInstrumentByIdAsync(id, cancellationToken);
                if (instrument == null)
                {
                    return NotFound($"Instrument with ID {id} not found.");
                }

                await _instrumentPresenter.DeleteInstrumentAsync(instrument, cancellationToken);
                return NoContent(); // Успешное удаление
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Получение списка всех инструментов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instrument>>> GetAllInstruments(CancellationToken cancellationToken)
        {
            try
            {
                var instruments = await _instrumentPresenter.GetInstrumentsAsync(cancellationToken);
                return Ok(instruments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Получение инструмента по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Instrument>> GetInstrumentById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var instrument = await _instrumentPresenter.GetInstrumentByIdAsync(id, cancellationToken);
                if (instrument == null)
                {
                    return NotFound($"Instrument with ID {id} not found.");
                }

                return Ok(instrument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
