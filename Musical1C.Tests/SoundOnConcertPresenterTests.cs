using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class SoundOnConcertPresenterTests
    {
        private Mock<ISoundOnConcertStorage> _mockStorage;
        private SoundOnConcertPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockStorage = new Mock<ISoundOnConcertStorage>();
            _presenter = new SoundOnConcertPresenter(_mockStorage.Object);
        }

        [Test]
        public async Task AddSoundOnConcertAsync_ShouldCallAddSoundOnConcertAsync()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var soundId = Guid.NewGuid();
            var token = new CancellationToken();

            // Act
            await _presenter.AddSoundOnConcertAsync(concertId, soundId, token);

            // Assert
            _mockStorage.Verify(
                storage => storage.AddSoundOnConcertAsync(
                    It.Is<SoundOnConcert>(s => s.ConcertId == concertId && s.SoundId == soundId),
                    token),
                Times.Once);
        }

        [Test]
        public async Task GetSoundOnConcertAsync_ShouldReturnAllSoundOnConcert()
        {
            // Arrange
            var expectedSounds = new List<SoundOnConcert>
            {
                new SoundOnConcert(Guid.NewGuid(), Guid.NewGuid()),
                new SoundOnConcert(Guid.NewGuid(), Guid.NewGuid())
            };
            var token = new CancellationToken();

            _mockStorage
                .Setup(storage => storage.GetAllSoundOnConcertAsync(token))
                .ReturnsAsync(expectedSounds);

            // Act
            var result = await _presenter.GetSoundOnConcertAsync(token);

            // Assert
            Assert.AreEqual(expectedSounds, result);
            _mockStorage.Verify(storage => storage.GetAllSoundOnConcertAsync(token), Times.Once);
        }
    }
}
