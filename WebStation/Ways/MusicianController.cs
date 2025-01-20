using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using WebStation.Trains;

namespace WebStation.Ways
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianController : ControllerBase
    {
        private readonly IMusicianPresenter _musicianPresenter;

        public MusicianController(IMusicianPresenter musicianPresenter)
        {
            _musicianPresenter = musicianPresenter;
        }

        // POST: api/Musician
        [HttpPost]
        public async Task<IActionResult> AddMusician([FromBody] AddMusicianRequest request, CancellationToken token)
        {
            if (request == null)
                return BadRequest("Invalid musician data.");

            // Преобразуем запрос в объекты для сохранения
            var instruments = new List<Instrument>();
            foreach (var instrumentRequest in request.Instruments)
            {
                instruments.Add(new Instrument { Id = instrumentRequest.Id, Name = instrumentRequest.Name });
            }

            var concerts = new List<Concert>();
            foreach (var concertRequest in request.Concerts)
            {
                concerts.Add(new Concert { Id = concertRequest.Id, Name = concertRequest.Name });
            }

            // Добавляем музыканта
            var musician = await _musicianPresenter.AddMusicianAsync(request.Name, request.LastName, request.Surname, instruments, concerts, token);

            return CreatedAtAction(nameof(GetMusicianById), new { id = musician.Id }, musician);
        }

        // GET: api/Musician/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicianById(Guid id, CancellationToken token)
        {
            var musician = await _musicianPresenter.GetMusicianByIdAsync(id, token);

            if (musician == null)
                return NotFound();

            var response = new MusicianResponse
            {
                Id = musician.Id,
                Name = musician.Name,
                LastName = musician.Lastname,
                Surname = musician.Surname,
                Instruments = new List<InstrumentResponse>(),
                Concerts = new List<ConcertResponse>()
            };

            foreach (var musicianInstrument in musician.MusicianInstruments)
            {
                response.Instruments.Add(new InstrumentResponse
                {
                    Id = musicianInstrument.Instrument.Id,
                    Name = musicianInstrument.Instrument.Name
                });
            }

            foreach (var musicianOnConcert in musician.MusicianOnConcerts)
            {
                response.Concerts.Add(new ConcertResponse
                {
                    Id = musicianOnConcert.Concert.Id,
                    Name = musicianOnConcert.Concert.Name
                });
            }

            return Ok(response);
        }

        // GET: api/Musician
        [HttpGet]
        public async Task<IActionResult> GetMusicians(CancellationToken token)
        {
            var musicians = await _musicianPresenter.GetMusiciansAsync(token);

            var response = new List<MusicianResponse>();

            foreach (var musician in musicians)
            {
                var musicianResponse = new MusicianResponse
                {
                    Id = musician.Id,
                    Name = musician.Name,
                    LastName = musician.Lastname,
                    Surname = musician.Surname,
                    Instruments = new List<InstrumentResponse>(),
                    Concerts = new List<ConcertResponse>()
                };

                foreach (var musicianInstrument in musician.MusicianInstruments)
                {
                    musicianResponse.Instruments.Add(new InstrumentResponse
                    {
                        Id = musicianInstrument.Instrument.Id,
                        Name = musicianInstrument.Instrument.Name
                    });
                }

                foreach (var musicianOnConcert in musician.MusicianOnConcerts)
                {
                    musicianResponse.Concerts.Add(new ConcertResponse
                    {
                        Id = musicianOnConcert.Concert.Id,
                        Name = musicianOnConcert.Concert.Name
                    });
                }

                response.Add(musicianResponse);
            }

            return Ok(response);
        }

        // GET: api/Musician/search?lastname={lastName}
        [HttpGet("search")]
        public async Task<IActionResult> SearchMusiciansByLastName([FromQuery] string lastName, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                return BadRequest("Last name is required.");

            var musicians = await _musicianPresenter.SearchMusiciansByLastNameAsync(lastName, token);

            var response = new List<MusicianResponse>();

            foreach (var musician in musicians)
            {
                response.Add(new MusicianResponse
                {
                    Id = musician.Id,
                    Name = musician.Name,
                    LastName = musician.Lastname,
                    Surname = musician.Surname
                });
            }

            return Ok(response);
        }

        // DELETE: api/Musician/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusician(Guid id, CancellationToken token)
        {
            var musician = await _musicianPresenter.GetMusicianByIdAsync(id, token);
            if (musician == null)
                return NotFound();

            await _musicianPresenter.DeleteMusicianAsync(musician, token);

            return NoContent();
        }
    }
}
