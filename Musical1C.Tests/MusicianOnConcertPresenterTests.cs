using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianOnConcertPresenterTests
    {
        private Mock<IMusicianOnConcertStorage> _mockStorage;
        private MusicianOnConcertPresenter _presenter;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            _mockStorage = new Mock<IMusicianOnConcertStorage>();
            _presenter = new MusicianOnConcertPresenter(_mockStorage.Object);
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task AddMusicianOnConcertAsync_ShouldCallAddMusicianOnConcertAsyncOnce()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var musicianId = Guid.NewGuid();
            var musicianOnConcert = new MusicianOnConcert(concertId, musicianId);

            // Act
            await _presenter.AddMusicianOnConcertAsync(concertId, musicianId, _cancellationToken);

            // Assert
            _mockStorage.Verify(
                storage => storage.AddMusicianOnConcertAsync(
                    It.Is<MusicianOnConcert>(moc => moc.ConcertId == concertId && moc.MusicianId == musicianId),
                    _cancellationToken),
                Times.Once);
        }

        [Test]
        public async Task GetMusicianOnConcertAsync_ShouldCallGetAllMusicianOnConcertAsyncOnce()
        {
            // Arrange
            var musicianOnConcertList = new List<MusicianOnConcert>
            {
                new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid()),
                new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid())
            };

            _mockStorage
                .Setup(storage => storage.GetAllMusicianOnConcertAsync(_cancellationToken))
                .ReturnsAsync(musicianOnConcertList);

            // Act
            var result = await _presenter.GetMusicianOnConcertAsync(_cancellationToken);

            // Assert
            Assert.AreEqual(musicianOnConcertList, result);
            _mockStorage.Verify(storage => storage.GetAllMusicianOnConcertAsync(_cancellationToken), Times.Once);
        }
    }
}
