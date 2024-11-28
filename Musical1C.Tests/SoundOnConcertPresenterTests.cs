using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class SoundOnConcertPresenterTests
    {
        private Mock<IStorageDataBase<SoundOnConcert>> _mockStorage;
        private SoundOnConcertPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockStorage = new Mock<IStorageDataBase<SoundOnConcert>>();
            _presenter = new SoundOnConcertPresenter(_mockStorage.Object);
        }

        [Test]
        public async Task AddSoundOnConcertAsync_ValidInput_ShouldCallAddAsync()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var soundId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act
            await _presenter.AddSoundOnConcertAsync(concertId, soundId, token);

            // Assert
            _mockStorage.Verify(s => s.AddAsync(It.Is<SoundOnConcert>(
                soc => soc.ConcertId == concertId && soc.SoundId == soundId), token), Times.Once);
        }

        [Test]
        public void AddSoundOnConcertAsync_EmptyConcertId_ShouldThrowArgumentNullException()
        {
            // Arrange
            var soundId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _presenter.AddSoundOnConcertAsync(Guid.Empty, soundId, token));
            Assert.AreEqual("concertId", ex.ParamName);
        }

        [Test]
        public void AddSoundOnConcertAsync_EmptySoundId_ShouldThrowArgumentNullException()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _presenter.AddSoundOnConcertAsync(concertId, Guid.Empty, token));
            Assert.AreEqual("soundId", ex.ParamName);
        }

        [Test]
        public async Task GetSoundOnConcertAsync_ShouldReturnListFromStorage()
        {
            // Arrange
            var token = CancellationToken.None;
            var soundOnConcerts = new List<SoundOnConcert>
            {
                new SoundOnConcert(Guid.NewGuid(), Guid.NewGuid()),
                new SoundOnConcert(Guid.NewGuid(), Guid.NewGuid())
            };

            _mockStorage
                .Setup(s => s.GetListAsync(null, null, token))
                .ReturnsAsync(soundOnConcerts);

            // Act
            var result = await _presenter.GetSoundOnConcertAsync(token);

            // Assert
            Assert.AreEqual(soundOnConcerts, result);
            _mockStorage.Verify(s => s.GetListAsync(null, null, token), Times.Once);
        }
    }
}
