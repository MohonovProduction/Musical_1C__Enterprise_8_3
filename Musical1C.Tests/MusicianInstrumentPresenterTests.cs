using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianInstrumentPresenterTests
    {
        private Mock<IStorageDataBase<MusicianInstrument>> _mockStorage;
        private MusicianInstrumentPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockStorage = new Mock<IStorageDataBase<MusicianInstrument>>();
            _presenter = new MusicianInstrumentPresenter(_mockStorage.Object);
        }

        [Test]
        public async Task AddMusicianInstrumentAsync_ShouldCallAddAsync()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var instrumentId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act
            await _presenter.AddMusicianInstrumentAsync(musicianId, instrumentId, token);

            // Assert
            _mockStorage.Verify(s => s.AddAsync(It.Is<MusicianInstrument>(
                mi => mi.MusicianId == musicianId && mi.InstrumentId == instrumentId), token), Times.Once);
        }

        [Test]
        public async Task GetMusicianInstrumentAsync_ShouldReturnListFromStorage()
        {
            // Arrange
            var token = CancellationToken.None;
            var musicianInstruments = new List<MusicianInstrument>
            {
                new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid()),
                new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid())
            };

            _mockStorage
                .Setup(s => s.GetListAsync(null, null, token))
                .ReturnsAsync(musicianInstruments);

            // Act
            var result = await _presenter.GetMusicianInstrumentAsync(token);

            // Assert
            Assert.AreEqual(musicianInstruments, result);
            _mockStorage.Verify(s => s.GetListAsync(null, null, token), Times.Once);
        }

        [Test]
        public async Task DeleteMusicianInstrumentAsync_ShouldCallDeleteAsync()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var instrumentId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act
            await _presenter.DeleteMusicianInstrumentAsync(musicianId, instrumentId, token);

            // Assert
            _mockStorage.Verify(s => s.DeleteAsync(
                $"MusicianId = {musicianId} AND InstrumentId = {instrumentId}", null, token), Times.Once);
        }
    }
}
