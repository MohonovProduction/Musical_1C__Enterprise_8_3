using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class ConcertPresenterTests
    {
        private Mock<IStorageDataBase<Concert>> _mockConcertStorage;
        private Mock<IStorageDataBase<MusicianOnConcert>> _mockMusicianOnConcertStorage;
        private Mock<IStorageDataBase<SoundOnConcert>> _mockSoundOnConcertStorage;
        private ConcertPresenter _concertPresenter;

        [SetUp]
        public void Setup()
        {
            _mockConcertStorage = new Mock<IStorageDataBase<Concert>>();
            _mockMusicianOnConcertStorage = new Mock<IStorageDataBase<MusicianOnConcert>>();
            _mockSoundOnConcertStorage = new Mock<IStorageDataBase<SoundOnConcert>>();
            _concertPresenter = new ConcertPresenter(
                _mockConcertStorage.Object, 
                _mockMusicianOnConcertStorage.Object, 
                _mockSoundOnConcertStorage.Object);
        }

        [Test]
        public async Task AddConcertAsync_ShouldAddConcert_WithMusiciansAndSounds()
        {
            // Arrange
            var concertName = "Concert 1";
            var concertType = "Rock";
            var concertDate = "2025-01-01";
            var musicians = new List<Musician> { new Musician { Id = Guid.NewGuid() } };
            var sounds = new List<Sound> { new Sound { Id = Guid.NewGuid() } };
            var cancellationToken = CancellationToken.None;

            var concert = new Concert(Guid.NewGuid(), concertName, concertType, concertDate);
            
            _mockConcertStorage
                .Setup(s => s.AddAsync(It.IsAny<Concert>(), cancellationToken))
                .Returns(Task.CompletedTask);

            _mockMusicianOnConcertStorage
                .Setup(s => s.AddAsync(It.IsAny<MusicianOnConcert>(), cancellationToken))
                .Returns(Task.CompletedTask);

            _mockSoundOnConcertStorage
                .Setup(s => s.AddAsync(It.IsAny<SoundOnConcert>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _concertPresenter.AddConcertAsync(concertName, concertType, concertDate, musicians, sounds, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            _mockConcertStorage.Verify(s => s.AddAsync(It.IsAny<Concert>(), cancellationToken), Times.Once);
            _mockMusicianOnConcertStorage.Verify(s => s.AddAsync(It.IsAny<MusicianOnConcert>(), cancellationToken), Times.Once);
            _mockSoundOnConcertStorage.Verify(s => s.AddAsync(It.IsAny<SoundOnConcert>(), cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteConcertAsync_ShouldRemoveConcert_WithMusiciansAndSounds()
        {
            // Arrange
            var concert = new Concert(Guid.NewGuid(), "Concert to delete", "Rock", "2025-01-01");
            var cancellationToken = CancellationToken.None;

            _mockConcertStorage
                .Setup(s => s.DeleteAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), cancellationToken))
                .Returns(Task.CompletedTask);

            _mockMusicianOnConcertStorage
                .Setup(s => s.DeleteAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), cancellationToken))
                .Returns(Task.CompletedTask);

            _mockSoundOnConcertStorage
                .Setup(s => s.DeleteAsync(It.IsAny<Func<IQueryable<SoundOnConcert>, IQueryable<SoundOnConcert>>>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _concertPresenter.DeleteConcertAsync(concert, cancellationToken);

            // Assert
            _mockConcertStorage.Verify(s => s.DeleteAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), cancellationToken), Times.Once);
            _mockMusicianOnConcertStorage.Verify(s => s.DeleteAsync(It.IsAny<Func<IQueryable<MusicianOnConcert>, IQueryable<MusicianOnConcert>>>(), cancellationToken), Times.Once);
            _mockSoundOnConcertStorage.Verify(s => s.DeleteAsync(It.IsAny<Func<IQueryable<SoundOnConcert>, IQueryable<SoundOnConcert>>>(), cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetConcertsAsync_ShouldReturnConcertsWithDetails()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var concerts = new List<Concert> {
                new Concert(Guid.NewGuid(), "Concert 1", "Rock", "2025-01-01"),
                new Concert(Guid.NewGuid(), "Concert 2", "Classical", "2025-02-01")
            };

            _mockConcertStorage
                .Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), cancellationToken))
                .ReturnsAsync(concerts);

            // Act
            var result = await _concertPresenter.GetConcertsAsync(cancellationToken);

            // Assert
            Assert.AreEqual(2, result.Count);
            _mockConcertStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetConcertByIdAsync_ShouldReturnCorrectConcert()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var concert = new Concert(concertId, "Concert 1", "Rock", "2025-01-01");
            var cancellationToken = CancellationToken.None;

            _mockConcertStorage
                .Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), cancellationToken))
                .ReturnsAsync(concert);

            // Act
            var result = await _concertPresenter.GetConcertByIdAsync(concertId, cancellationToken);

            // Assert
            Assert.AreEqual(concertId, result.Id);
            _mockConcertStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Concert>, IQueryable<Concert>>>(), cancellationToken), Times.Once);
        }
    }
}
