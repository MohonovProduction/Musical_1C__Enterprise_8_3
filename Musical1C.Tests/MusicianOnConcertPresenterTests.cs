using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianOnConcertPresenterTests
    {
        private Mock<IStorageDataBase<MusicianOnConcert>> _mockStorage;
        private MusicianOnConcertPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockStorage = new Mock<IStorageDataBase<MusicianOnConcert>>();
            _presenter = new MusicianOnConcertPresenter(_mockStorage.Object);
        }

        [Test]
        public async Task AddMusicianOnConcertAsync_ShouldCallAddAsync()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var musicianId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act
            await _presenter.AddMusicianOnConcertAsync(concertId, musicianId, token);

            // Assert
            _mockStorage.Verify(s => s.AddAsync(It.Is<MusicianOnConcert>(
                moc => moc.ConcertId == concertId && moc.MusicianId == musicianId), token), Times.Once);
        }

        [Test]
        public async Task GetMusicianOnConcertAsync_ShouldReturnListFromStorage()
        {
            // Arrange
            var token = CancellationToken.None;
            var musicianOnConcerts = new List<MusicianOnConcert>
            {
                new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid()),
                new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid())
            };

            _mockStorage
                .Setup(s => s.GetListAsync(null, null, token))
                .ReturnsAsync(musicianOnConcerts);

            // Act
            var result = await _presenter.GetMusicianOnConcertAsync(token);

            // Assert
            Assert.AreEqual(musicianOnConcerts, result);
            _mockStorage.Verify(s => s.GetListAsync(null, null, token), Times.Once);
        }

        [Test]
        public async Task DeleteMusicianOnConcertAsync_ShouldCallDeleteAsync()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var musicianId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act
            await _presenter.DeleteMusicianOnConcertAsync(concertId, musicianId, token);

            // Assert
            _mockStorage.Verify(s => s.DeleteAsync(
                $"ConcertId = {concertId} AND MusicianId = {musicianId}", null, token), Times.Once);
        }
    }
}
