using Moq;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class InstrumentStorageTests
    {
        private Mock<IStorageFile<Instrument>> _mockStorageFile;
        private InstrumentStorage _instrumentStorage;

        [SetUp]
        public void SetUp()
        {
            _mockStorageFile = new Mock<IStorageFile<Instrument>>();
            _instrumentStorage = new InstrumentStorage(_mockStorageFile.Object)
            {
                // Заменяем внутренний объект StorageFile на мок
                _storageFile = _mockStorageFile.Object
            };
        }

        [Test]
        public async Task AddInstrumentAsync_ShouldCallAddAsync()
        {
            // Arrange
            var token = CancellationToken.None;
            var instrument = new Instrument(Guid.NewGuid(), "Guitar");

            // Act
            await _instrumentStorage.AddInstrumentAsync(instrument, token);

            // Assert
            _mockStorageFile.Verify(storage => storage.AddAsync(instrument, token), Times.Once);
        }

        [Test]
        public async Task DeleteInstrumentAsync_ShouldCallDeleteAsync()
        {
            // Arrange
            var token = CancellationToken.None;
            var instrumentId = Guid.NewGuid();
            var instrument = new Instrument(instrumentId, "Piano");

            // Act
            await _instrumentStorage.DeleteInstrumentAsync(instrument, token);

            // Assert
            _mockStorageFile.Verify(storage => storage.DeleteAsync(It.IsAny<Func<Instrument, bool>>(), token), Times.Once);
        }

        [Test]
        public async Task GetInstrumentsAsync_ShouldReturnInstruments()
        {
            // Arrange
            var token = CancellationToken.None;
            var instrumentsList = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Drum"),
                new Instrument(Guid.NewGuid(), "Violin")
            };

            _mockStorageFile.Setup(storage => storage.GetAllAsync(token)).ReturnsAsync(instrumentsList);

            // Act
            var result = await _instrumentStorage.GetInstrumentsAsync(token);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains(result.First(i => i.Name == "Drum"), result.ToList());
            Assert.Contains(result.First(i => i.Name == "Violin"), result.ToList());
        }
    }
}