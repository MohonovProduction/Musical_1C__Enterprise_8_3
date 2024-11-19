using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianInstrumentPresenterTests
    {
        private Mock<IMusicianInstrumentStorage> _mockInstrumentStorage;
        private MusicianInstrumentPresenter _presenter;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            _mockInstrumentStorage = new Mock<IMusicianInstrumentStorage>();
            _presenter = new MusicianInstrumentPresenter(_mockInstrumentStorage.Object);
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task AddMusicianInstrumentAsync_ShouldCallAddMusicianInstrumentAsyncOnce()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var instrumentId = Guid.NewGuid();
            var musicianInstrument = new MusicianInstrument(musicianId, instrumentId);

            // Act
            await _presenter.AddMusicianInstrumentAsync(musicianId, instrumentId, _cancellationToken);

            // Assert
            _mockInstrumentStorage.Verify(
                storage => storage.AddMusicianInstrumentAsync(
                    It.Is<MusicianInstrument>(mi => mi.MusicianId == musicianId && mi.InstrumentId == instrumentId),
                    _cancellationToken),
                Times.Once);
        }

        [Test]
        public async Task GetMusicianInstrumentAsync_ShouldCallGetAllMusicianInstrumentAsyncOnce()
        {
            // Arrange
            var musicianInstruments = new List<MusicianInstrument>
            {
                new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid()),
                new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid())
            };

            _mockInstrumentStorage
                .Setup(storage => storage.GetAllMusicianInstrumentAsync(_cancellationToken))
                .ReturnsAsync(musicianInstruments);

            // Act
            var result = await _presenter.GetMusicianInstrumentAsync(_cancellationToken);

            // Assert
            Assert.AreEqual(musicianInstruments, result);
            _mockInstrumentStorage.Verify(storage => storage.GetAllMusicianInstrumentAsync(_cancellationToken), Times.Once);
        }
    }
}
