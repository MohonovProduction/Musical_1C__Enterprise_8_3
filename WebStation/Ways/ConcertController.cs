using Microsoft.AspNetCore.Mvc;
using Presenter;
using Storage;
using WebStation.Trains;

namespace WebStation.Ways;

[Route("api/[controller]")]
[ApiController]
public class ConcertController : ControllerBase
{
    private readonly IConcertPresenter _concertPresenter;

    public ConcertController(IConcertPresenter concertPresenter)
    {
        _concertPresenter = concertPresenter;
    }

    // POST: api/Concert
    [HttpPost]
    public async Task<IActionResult> AddConcert([FromBody] AddConcertRequest request, CancellationToken token)
    {
        if (request == null)
            return BadRequest("Invalid concert data.");

        // Преобразуем запрос в данные, необходимые для создания концерта
        var musicians = new List<Musician>();
        foreach (var musicianRequest in request.Musicians)
        {
            musicians.Add(new Musician
            {
                Id = musicianRequest.Id,
                Name = musicianRequest.Name,
                Lastname = musicianRequest.Lastname,
                Surname = musicianRequest.Surname
            });
        }

        var sounds = new List<Sound>();
        foreach (var soundRequest in request.Sounds)
        {
            sounds.Add(new Sound
            {
                Id = soundRequest.Id,
                Name = soundRequest.Name,
                Author = soundRequest.Author
            });
        }

        // Добавляем концерт
        var concert = await _concertPresenter.AddConcertAsync(request.Name, request.Type, request.Date, musicians, sounds, token);

        return CreatedAtAction(nameof(GetConcertById), new { id = concert.Id }, concert);
    }

    // GET: api/Concert/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConcertById(Guid id, CancellationToken token)
    {
        var concert = await _concertPresenter.GetConcertByIdAsync(id, token);

        if (concert == null)
            return NotFound();

        var response = new ConcertResponse
        {
            Id = concert.Id,
            Name = concert.Name,
            Type = concert.Type,
            Date = concert.Date,
            Musicians = new List<MusicianResponse>(),
            Sounds = new List<SoundResponse>()
        };

        foreach (var musician in concert.MusicianOnConcerts)
        {
            response.Musicians.Add(new MusicianResponse
            {
                Id = musician.Musician.Id,
                Name = musician.Musician.Name,
                LastName = musician.Musician.Lastname,
                Surname = musician.Musician.Surname
            });
        }

        foreach (var sound in concert.SoundOnConcerts)
        {
            response.Sounds.Add(new SoundResponse
            {
                Id = sound.Sound.Id,
                Name = sound.Sound.Name,
                Author = sound.Sound.Author
            });
        }

        return Ok(response);
    }

    // GET: api/Concert
    [HttpGet]
    public async Task<IActionResult> GetConcerts(CancellationToken token)
    {
        var concerts = await _concertPresenter.GetConcertsAsync(token);

        var response = new List<ConcertResponse>();

        foreach (var concert in concerts)
        {
            var concertResponse = new ConcertResponse
            {
                Id = concert.Id,
                Name = concert.Name,
                Type = concert.Type,
                Date = concert.Date,
                Musicians = new List<MusicianResponse>(),
                Sounds = new List<SoundResponse>()
            };

            foreach (var musician in concert.MusicianOnConcerts)
            {
                concertResponse.Musicians.Add(new MusicianResponse
                {
                    Id = musician.Musician.Id,
                    Name = musician.Musician.Name,
                    LastName = musician.Musician.Lastname,
                    Surname = musician.Musician.Surname
                });
            }

            foreach (var sound in concert.SoundOnConcerts)
            {
                concertResponse.Sounds.Add(new SoundResponse
                {
                    Id = sound.Sound.Id,
                    Name = sound.Sound.Name,
                    Author = sound.Sound.Author
                });
            }

            response.Add(concertResponse);
        }

        return Ok(response);
    }

    // PUT: api/Concert/{id}/musician
    [HttpPut("{id}/musician")]
    public async Task<IActionResult> AddMusicianToConcert(Guid id, [FromBody] AddMusicianToConcertRequest request, CancellationToken token)
    {
        var concert = await _concertPresenter.GetConcertByIdAsync(id, token);
        if (concert == null)
            return NotFound();

        var musician = new Musician { Id = request.MusicianId };
        await _concertPresenter.AddMusicianToConcertAsync(concert, musician, token);

        return NoContent();
    }

    // PUT: api/Concert/{id}/sound
    [HttpPut("{id}/sound")]
    public async Task<IActionResult> AddSoundToConcert(Guid id, [FromBody] AddSoundToConcertRequest request, CancellationToken token)
    {
        var concert = await _concertPresenter.GetConcertByIdAsync(id, token);
        if (concert == null)
            return NotFound();

        var sound = new Sound { Id = request.SoundId };
        await _concertPresenter.AddSoundToConcertAsync(concert, sound, token);

        return NoContent();
    }

    // DELETE: api/Concert/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConcert(Guid id, CancellationToken token)
    {
        var concert = await _concertPresenter.GetConcertByIdAsync(id, token);
        if (concert == null)
            return NotFound();

        await _concertPresenter.DeleteConcertAsync(concert, token);

        return NoContent();
    }

    // DELETE: api/Concert/{id}/musician
    [HttpDelete("{id}/musician")]
    public async Task<IActionResult> RemoveMusicianFromConcert(Guid id, [FromBody] RemoveMusicianFromConcertRequest request, CancellationToken token)
    {
        var concert = await _concertPresenter.GetConcertByIdAsync(id, token);
        if (concert == null)
            return NotFound();

        var musician = new Musician { Id = request.MusicianId };
        await _concertPresenter.RemoveMusicianFromConcertAsync(concert, musician, token);

        return NoContent();
    }

    // DELETE: api/Concert/{id}/sound
    [HttpDelete("{id}/sound")]
    public async Task<IActionResult> RemoveSoundFromConcert(Guid id, [FromBody] RemoveSoundFromConcertRequest request, CancellationToken token)
    {
        var concert = await _concertPresenter.GetConcertByIdAsync(id, token);
        if (concert == null)
            return NotFound();

        var sound = new Sound { Id = request.SoundId };
        await _concertPresenter.RemoveSoundFromConcertAsync(concert, sound, token);

        return NoContent();
    }
}