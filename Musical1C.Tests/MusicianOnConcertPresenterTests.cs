using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianOnConcertPresenterTests
    {
        private Mock<IStorageDataBase<MusicianOnConcert>> _mockMusicianOnConcertStorage;
        private Mock<IStorageDataBase<Musician>> _mockMusicianStorage;
        private Mock<IStorageDataBase<Concert>> _mockConcertStorage;
        private MusicianOnConcertPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockMusicianOnConcertStorage = new Mock<IStorageDataBase<MusicianOnConcert>>();
            _mockMusicianStorage = new Mock<IStorageDataBase<Musician>>();
            _mockConcertStorage = new Mock<IStorageDataBase<Concert>>();
            _presenter = new MusicianOnConcertPresenter(_mockMusicianOnConcertStorage.Object, 
                                                        _mockMusicianStorage.Object, 
                                                        _mockConcertStorage.Object);
        }

        [Test]
        public async Task AddMusicianOnConcertAsync_ShouldAddMusicianToConcert()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var musicianId = Guid.NewGuid();
            var musician = new Musician(musicianId, "John", "Doe", "Smith");
            var concert = new Concert(concertId, "Rock Concert", "Rock", "2025-01-01");
            var token = CancellationToken.None;

            _mockMusicianStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync(musician);
            _mockConcertStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), token))
                .ReturnsAsync(concert);

            // Act
            await _presenter.AddMusicianOnConcertAsync(concertId, musicianId, token);

            // Assert
            _mockMusicianStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token), Times.Once);
            _mockConcertStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), token), Times.Once);
            _mockMusicianOnConcertStorage.Verify(s => s.AddAsync(It.Is<MusicianOnConcert>(
                moc => moc.ConcertId == concertId && moc.MusicianId == musicianId), token), Times.Once);
        }

        [Test]
        public void AddMusicianOnConcertAsync_ShouldThrowArgumentException_WhenMusicianOrConcertNotFound()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var musicianId = Guid.NewGuid();
            var token = CancellationToken.None;

            _mockMusicianStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync((Musician)null);
            _mockConcertStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), token))
                .ReturnsAsync((Concert)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.AddMusicianOnConcertAsync(concertId, musicianId, token));

            Assert.That(exception.Message, Is.EqualTo("Musician or concert not found."));
        }

        [Test]
        public async Task DeleteMusicianOnConcertAsync_ShouldDeleteMusicianFromConcert()
        {
            // Arrange
            var musicianOnConcert = new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid());
            var token = CancellationToken.None;

            // Act
            await _presenter.DeleteMusicianOnConcertAsync(musicianOnConcert, token);

            // Assert
            _mockMusicianOnConcertStorage.Verify(s => s.DeleteAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusiciansOnConcertAsync_ShouldReturnListOfMusiciansForConcert()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var musicians = new List<Musician>
            {
                new Musician(Guid.NewGuid(), "John", "Doe", "Smith"),
                new Musician(Guid.NewGuid(), "Jane", "Doe", "Smith")
            };
            var musicianOnConcerts = musicians.Select(m => new MusicianOnConcert(concertId, m.Id)).ToList();
            var token = CancellationToken.None;

            _mockMusicianOnConcertStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token))
                .ReturnsAsync(musicianOnConcerts);
            _mockMusicianStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync(musicians);

            // Act
            var result = await _presenter.GetMusiciansOnConcertAsync(concertId, token);

            // Assert
            Assert.AreEqual(musicians.Count, result.Count);
            _mockMusicianOnConcertStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetConcertsForMusicianAsync_ShouldReturnListOfConcertsForMusician()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var concerts = new List<Concert>
            {
                new Concert(Guid.NewGuid(), "Rock Concert", "Rock", "2025-01-01"),
                new Concert(Guid.NewGuid(), "Jazz Concert", "Jazz", "2025-02-01")
            };
            var musicianOnConcerts = concerts.Select(c => new MusicianOnConcert(c.Id, musicianId)).ToList();
            var token = CancellationToken.None;

            _mockMusicianOnConcertStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token))
                .ReturnsAsync(musicianOnConcerts);
            _mockConcertStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), token))
                .ReturnsAsync(concerts);

            // Act
            var result = await _presenter.GetConcertsForMusicianAsync(musicianId, token);

            // Assert
            Assert.AreEqual(concerts.Count, result.Count);
            _mockMusicianOnConcertStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusiciansOnConcertsAsync_ShouldReturnListOfMusiciansOnConcerts()
        {
            // Arrange
            var musicianOnConcerts = new List<MusicianOnConcert>
            {
                new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid()),
                new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid())
            };
            var token = CancellationToken.None;

            _mockMusicianOnConcertStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token))
                .ReturnsAsync(musicianOnConcerts);

            // Act
            var result = await _presenter.GetMusiciansOnConcertsAsync(token);

            // Assert
            Assert.AreEqual(musicianOnConcerts.Count, result.Count);
            _mockMusicianOnConcertStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusicianOnConcertByIdsAsync_ShouldReturnMusicianOnConcert()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var musicianId = Guid.NewGuid();
            var musicianOnConcert = new MusicianOnConcert(concertId, musicianId);
            var token = CancellationToken.None;

            _mockMusicianOnConcertStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token))
                .ReturnsAsync(musicianOnConcert);

            // Act
            var result = await _presenter.GetMusicianOnConcertByIdsAsync(concertId, musicianId, token);

            // Assert
            Assert.AreEqual(musicianOnConcert.ConcertId, result.ConcertId);
            Assert.AreEqual(musicianOnConcert.MusicianId, result.MusicianId);
            _mockMusicianOnConcertStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), token), Times.Once);
        }
    }
}
