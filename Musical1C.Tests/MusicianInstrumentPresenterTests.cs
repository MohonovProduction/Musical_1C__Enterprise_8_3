using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianInstrumentPresenterTests
    {
        private Mock<IStorageDataBase<MusicianInstrument>> _mockMusicianInstrumentStorage;
        private Mock<IStorageDataBase<Musician>> _mockMusicianStorage;
        private Mock<IStorageDataBase<Instrument>> _mockInstrumentStorage;
        private MusicianInstrumentPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockMusicianInstrumentStorage = new Mock<IStorageDataBase<MusicianInstrument>>();
            _mockMusicianStorage = new Mock<IStorageDataBase<Musician>>();
            _mockInstrumentStorage = new Mock<IStorageDataBase<Instrument>>();
            _presenter = new MusicianInstrumentPresenter(_mockMusicianInstrumentStorage.Object, 
                                                        _mockMusicianStorage.Object, 
                                                        _mockInstrumentStorage.Object);
        }

        [Test]
        public async Task AddMusicianInstrumentAsync_ShouldAddInstrumentToMusician()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var instrumentId = Guid.NewGuid();
            var musician = new Musician(musicianId, "John", "Doe", "Smith");
            var instrument = new Instrument(instrumentId, "Guitar");
            var token = CancellationToken.None;

            _mockMusicianStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync(musician);
            _mockInstrumentStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Instrument>, IQueryable<Instrument>>>(), token))
                .ReturnsAsync(instrument);

            // Act
            await _presenter.AddMusicianInstrumentAsync(musicianId, instrumentId, token);

            // Assert
            _mockMusicianStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token), Times.Once);
            _mockInstrumentStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Instrument>, IQueryable<Instrument>>>(), token), Times.Once);
            _mockMusicianInstrumentStorage.Verify(s => s.AddAsync(It.Is<MusicianInstrument>(
                mi => mi.MusicianId == musicianId && mi.InstrumentId == instrumentId), token), Times.Once);
        }

        [Test]
        public void AddMusicianInstrumentAsync_ShouldThrowArgumentException_WhenMusicianOrInstrumentNotFound()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var instrumentId = Guid.NewGuid();
            var token = CancellationToken.None;

            _mockMusicianStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync((Musician)null);
            _mockInstrumentStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Instrument>, IQueryable<Instrument>>>(), token))
                .ReturnsAsync((Instrument)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.AddMusicianInstrumentAsync(musicianId, instrumentId, token));

            Assert.That(exception.Message, Is.EqualTo("Musician or instrument not found."));
        }

        [Test]
        public async Task DeleteMusicianInstrumentAsync_ShouldDeleteInstrumentFromMusician()
        {
            // Arrange
            var musicianInstrument = new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid(), null, null);
            var token = CancellationToken.None;

            // Act
            await _presenter.DeleteMusicianInstrumentAsync(musicianInstrument, token);

            // Assert
            _mockMusicianInstrumentStorage.Verify(s => s.DeleteAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetInstrumentsForMusicianAsync_ShouldReturnListOfInstrumentsForMusician()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var instruments = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Guitar"),
                new Instrument(Guid.NewGuid(), "Piano")
            };
            var musicianInstruments = instruments.Select(i => new MusicianInstrument(musicianId, i.Id, null, i)).ToList();
            var token = CancellationToken.None;

            _mockMusicianInstrumentStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token))
                .ReturnsAsync(musicianInstruments);
            _mockInstrumentStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<Instrument>, IQueryable<Instrument>>>(), token))
                .ReturnsAsync(instruments);

            // Act
            var result = await _presenter.GetInstrumentsForMusicianAsync(musicianId, token);

            // Assert
            Assert.AreEqual(instruments.Count, result.Count);
            _mockMusicianInstrumentStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusiciansForInstrumentAsync_ShouldReturnListOfMusiciansForInstrument()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var musicians = new List<Musician>
            {
                new Musician(Guid.NewGuid(), "John", "Doe", "Smith"),
                new Musician(Guid.NewGuid(), "Jane", "Doe", "Smith")
            };
            var musicianInstruments = musicians.Select(m => new MusicianInstrument(m.Id, instrumentId, m, null)).ToList();
            var token = CancellationToken.None;

            _mockMusicianInstrumentStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token))
                .ReturnsAsync(musicianInstruments);
            _mockMusicianStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync(musicians);

            // Act
            var result = await _presenter.GetMusiciansForInstrumentAsync(instrumentId, token);

            // Assert
            Assert.AreEqual(musicians.Count, result.Count);
            _mockMusicianInstrumentStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusicianInstrumentsAsync_ShouldReturnListOfMusicianInstruments()
        {
            // Arrange
            var musicianInstruments = new List<MusicianInstrument>
            {
                new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid(), null, null),
                new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid(), null, null)
            };
            var token = CancellationToken.None;

            _mockMusicianInstrumentStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token))
                .ReturnsAsync(musicianInstruments);

            // Act
            var result = await _presenter.GetMusicianInstrumentsAsync(token);

            // Assert
            Assert.AreEqual(musicianInstruments.Count, result.Count);
            _mockMusicianInstrumentStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusicianInstrumentByIdsAsync_ShouldReturnMusicianInstrument()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var instrumentId = Guid.NewGuid();
            var musicianInstrument = new MusicianInstrument(musicianId, instrumentId, null, null);
            var token = CancellationToken.None;

            _mockMusicianInstrumentStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token))
                .ReturnsAsync(musicianInstrument);

            // Act
            var result = await _presenter.GetMusicianInstrumentByIdsAsync(musicianId, instrumentId, token);

            // Assert
            Assert.AreEqual(musicianInstrument.MusicianId, result.MusicianId);
            Assert.AreEqual(musicianInstrument.InstrumentId, result.InstrumentId);
            _mockMusicianInstrumentStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token), Times.Once);
        }
    }
}
