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
    [ApiController]
    [Route("api/[controller]")]
    public class ConcertController : ControllerBase
    {
        private readonly IConcertPresenter _concertPresenter; // Используем интерфейс

        public ConcertController(IConcertPresenter concertPresenter) // Получаем через интерфейс
        {
            _concertPresenter = concertPresenter;
        }

        // GET: api/concert
        [HttpGet]
        public async Task<IActionResult> GetConcerts(CancellationToken cancellationToken)
        {
            try
            {
                var concerts = await _concertPresenter.GetConcertsAsync(cancellationToken);
                return Ok(concerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/concert
        [HttpPost]
        public async Task<IActionResult> AddConcert([FromBody] Concert concert, CancellationToken cancellationToken)
        {
            if (concert == null || string.IsNullOrEmpty(concert.Name) || string.IsNullOrEmpty(concert.Type) || string.IsNullOrEmpty(concert.Date))
            {
                return BadRequest("Invalid concert data.");
            }

            try
            {
                var success = await _concertPresenter.AddConcertAsync(concert.Name, cancellationToken);
                if (success)
                {
                    return CreatedAtAction(nameof(GetConcertById), new { id = concert.Id }, concert);
                }
                return BadRequest("Failed to add concert.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/concert/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConcertById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var concerts = await _concertPresenter.GetConcertsAsync(cancellationToken);
                var concert = concerts.FirstOrDefault(c => c.Id == id);
                if (concert == null)
                {
                    return NotFound();
                }

                return Ok(concert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/concert/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcert(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var concerts = await _concertPresenter.GetConcertsAsync(cancellationToken);
                var concert = concerts.FirstOrDefault(c => c.Id == id);
                if (concert == null)
                {
                    return NotFound();
                }

                await _concertPresenter.DeleteConcertAsync(concert, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
