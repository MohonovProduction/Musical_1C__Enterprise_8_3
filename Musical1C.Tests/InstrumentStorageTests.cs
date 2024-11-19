using Moq;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class InstrumentStorageTests
    {
        private Mock<IStorageDataBase<Instrument>> _mockStorageDataBase;
        private InstrumentStorage _instrumentStorage;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            _mockStorageDataBase = new Mock<IStorageDataBase<Instrument>>();
            _instrumentStorage = new InstrumentStorage(_mockStorageDataBase.Object);
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task AddInstrumentAsync_ShouldCallAddAsyncOnce()
        {
            // Arrange
            var instrument = new Instrument { Id = Guid.NewGuid(), Name = "Guitar" };

            // Act
            await _instrumentStorage.AddInstrumentAsync(instrument, _cancellationToken);

            // Assert
            _mockStorageDataBase.Verify(db => db.AddAsync(instrument, _cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteInstrumentAsync_ShouldCallDeleteAsyncWithCorrectParameters()
        {
            // Arrange
            var instrument = new Instrument { Id = Guid.NewGuid(), Name = "Guitar" };

            // Act
            await _instrumentStorage.DeleteInstrumentAsync(instrument, _cancellationToken);

            // Assert
            _mockStorageDataBase.Verify(db =>
                db.DeleteAsync("id = @Id", It.Is<object>(p => (Guid)p.GetType().GetProperty("Id").GetValue(p) == instrument.Id),
                _cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetInstrumentsAsync_ShouldCallGetListAsyncOnce()
        {
            // Arrange
            var instruments = new List<Instrument>
            {
                new Instrument { Id = Guid.NewGuid(), Name = "Guitar" },
                new Instrument { Id = Guid.NewGuid(), Name = "Piano" }
            };

            _mockStorageDataBase
                .Setup(db => db.GetListAsync(It.IsAny<string>(), null, _cancellationToken))
                .ReturnsAsync(instruments);

            // Act
            var result = await _instrumentStorage.GetInstrumentsAsync(_cancellationToken);

            // Assert
            Assert.AreEqual(instruments, result);
            _mockStorageDataBase.Verify(db => db.GetListAsync(It.IsAny<string>(), null, _cancellationToken), Times.Once);
        }
    }
}
